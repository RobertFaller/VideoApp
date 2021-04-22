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
    public class metadataController : ControllerBase
    {
        //POST movie data to "Database"
        [HttpPost("/metadata")]
        public ActionResult metaData(MovieData content)
        {
            //Convert the Model to a string to output
            //if saving this to a Database ideally would 
            //be using something like EntityFramework
            string test = string.Concat(content.movieId + "," 
                                        + content.title + "," 
                                        + content.language + "," 
                                        + content.duration + "," 
                                        + content.duration
                                        );
            
            try{
                //Open the Streamreader for the local file - boolean added to allow amendments to the Database.txt
                using (StreamWriter Database = new StreamWriter("Database.txt", true))
                {
                    //Write the new entry out to the Database.txt file
                    Database.WriteLine(test);
                }
                //return success if able to write successfully
                return Ok("Movie Successfull Save");
            }
            catch
            {
                //return failed if request failed.
                return BadRequest("Failed to add Movie to Database");
            }
        }

        //Get Movie data
        [HttpGet("/metadata/{movieId:int}")]
        public ActionResult metadata(int movieId)
        {
            try{
                //build the list of data to work with from the provided .csv file
                List<MovieData> rawData = System.IO.File.ReadAllLines("..\\metadata.csv")
                                              .Skip(1)
                                              .Select(d => DataFromCsv.MovieDataFromCsv(d))
                                              .ToList();
                
                //initiate a new object to populate with the data for the output
                List<MovieData> movieData = new List<MovieData>();
                
                //Initial Linq query to filter down the results
                movieData = rawData.Where(x => x.movieId == movieId && x.id != -1).Select(x => x).ToList();

                //Group the results and then order by language
                movieData = movieData.GroupBy(x => x.language)
                                     .Select(x => x.OrderByDescending(x => x.id).First())
                                     .OrderBy(x => x.language)
                                     .ToList();
                
                //Convert to Dto for export
                List<movieDataDto> Result = movieData.Select(x => new movieDataDto{
                    movieId = x.movieId,
                    title = x.title,
                    language = x.language,
                    duration = x.duration,
                    releaseYear = x.releaseYear
                }
                ).ToList();


                //Check to make sure that the movie data exists, and if not return a 404
                if(Result == null)
                {
                    return BadRequest();
                }
                //if the movie data does exist, serialise to Json and then return the result
                else
                {
                    var JsonMovieData = JsonSerializer.Serialize<List<movieDataDto>>(Result);

                    return Ok(JsonMovieData);
                }
            }
                //catch in case there is an issue with reading the data, and if there is return a 404 with the innder exception.
            catch(Exception ex){
                return NotFound(ex.InnerException);
            }

        }
    }
}