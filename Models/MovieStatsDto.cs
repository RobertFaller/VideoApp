using System;

namespace VideoApp.Models
{
    public class MovieStatsDto
    {
        public int movieId { get; set; }

        public string title { get; set; }

        public Int64 averageWatchDurationS { get; set; }

        public Int64 watches { get; set; }

        public int releaseYear { get; set; }
    }
}