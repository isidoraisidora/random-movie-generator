using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace RandomMovieGenerator.Models
{
    public class Watchlist
    {
        public int Id { get; set; }
        public string UserId { get; set; }  // string if using Identity User Id
        public int MovieId { get; set; }

        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; }

        [ForeignKey("MovieId")]
        public virtual Movie Movie { get; set; }
    }
}