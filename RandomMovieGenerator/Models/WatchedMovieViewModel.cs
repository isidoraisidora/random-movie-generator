using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RandomMovieGenerator.Models
{
    public class WatchedMovieViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Genre { get; set; }
        public string ImageUrl { get; set; }
        public int ReleaseYear { get; set; }
        public string Director { get; set; }
        public int Duration { get; set; }
        public float Rating { get; set; }    
        public int? YourRating { get; set; } 
    }
}