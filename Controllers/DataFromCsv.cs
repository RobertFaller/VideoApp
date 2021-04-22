using System;
using System.Linq;
using System.Text.RegularExpressions;
using VideoApp.Models;

namespace VideoApp.Controllers
{
    public class DataFromCsv
    {
        public static MovieData MovieDataFromCsv(string csvLine)
        {

            string[] values = csvLine.Split(',');

            if(values.Count() > 6)
            {
                values = Regex.Split(csvLine, ",(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");
            }
            
            try{
            MovieData movieData = new MovieData();
            movieData.id = Convert.ToInt32(values[0]);
            movieData.movieId = Convert.ToInt32(values[1]);
            movieData.title = Convert.ToString(values[2]);
            movieData.language = Convert.ToString(values[3]);
            movieData.duration = Convert.ToString(values[4]);
            movieData.releaseYear = Convert.ToInt32(values[5]);
            return movieData;
            }
            catch(Exception ex)
            {
                return new MovieData(){ id = -1 };
            }
        }
        
        public static MovieStats MovieStatsFromCsv(string csvLine)
        {
            string[] values = csvLine.Split(',');

            if(values.Count() > 2)
            {
                values = Regex.Split(csvLine, ",(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");
            }
            try{
                MovieStats movieStats = new MovieStats();
                movieStats.movieId = Convert.ToInt32(values[0]);
                movieStats.averageWatchDurationS = Convert.ToInt32(values[1]);
                return movieStats;
            }
            catch(Exception ex)
            {
                return null;
            }
        }

    }
}