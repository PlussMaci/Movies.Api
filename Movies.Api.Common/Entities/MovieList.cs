using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Movies.Api.Common.Entities
{
    public class MovieList
    {
        public Guid ID { get; set; }

        [Required]
        public string Title { get; set; }

        public string Description { get; set; }

        public List<Movie> movies { get; set; }

        public Guid UserID { get; set; }
    }
}
