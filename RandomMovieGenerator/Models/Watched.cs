using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RandomMovieGenerator.Models
{
    public class Watched
    {
        public int Id { get; set; }
        public string UserId { get; set; }  
        public int MovieId { get; set; }

        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; }

        [ForeignKey("MovieId")]
        public virtual Movie Movie { get; set; }
        public int? Rating { get; set; }
    }
}