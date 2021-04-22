using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace VideoApp.Models
{
    public class movieDataDto
    {
        public int movieId { get; set; }

        public string title { get; set; }

        public string language { get; set; }

        public string duration { get; set; }

        public int releaseYear { get; set; }
    }
}