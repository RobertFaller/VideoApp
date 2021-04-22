using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using VideoApp.Models;

namespace VideoApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class moviesController : ControllerBase
    {
        [HttpGet("/movies/stats")]
        public ActionResult moviesStats()
        {
            //Collect the Data from the CSV Sources
            List<MovieData> rawData = System.IO.File.ReadAllLines("..\\metadata.csv")
                                      .Skip(1)
                                      .Select(d => DataFromCsv.MovieDataFromCsv(d))
                                      .ToList();
            List<MovieStats> rawStats = System.IO.File.ReadAllLines("..\\stats.csv")
                                        .Skip(1)
                                        .Select(d => DataFromCsv.MovieStatsFromCsv(d))
                                        .ToList();

            //refine data by grouping
            rawData = rawData.GroupBy(g => g.movieId).Select(x => x.OrderByDescending(x => x.movieId).First()).ToList();
            rawStats = rawStats.GroupBy(g => g.movieId)
                                .Select(x => new MovieStats
                                {
                                    movieId = x.First().movieId,
                                    averageWatchDurationS = x.Sum(c => c.averageWatchDurationS),
                                    count = x.Count()
                                }).ToList();
            
            //combine data for output Json
            List<MovieStatsDto> movieStatsDto = rawData.Join(rawStats, d => d.movieId, s => s.movieId, (d, s) => new MovieStatsDto{
                movieId = d.movieId,
                title = d.title,
                averageWatchDurationS = s.averageWatchDurationS,
                watches = (s.averageWatchDurationS / Convert.ToInt64(s.count)),
                releaseYear = d.releaseYear
            }).OrderByDescending(x => x.watches).ThenBy(x => x.releaseYear).ToList();

            //Covert output to Json
            var JsonMovieStats = JsonSerializer.Serialize<List<MovieStatsDto>>(movieStatsDto);
            
            //Return Json
            return Ok(JsonMovieStats);
        }
        
    }
}