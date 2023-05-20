using System;
using System.ComponentModel.DataAnnotations;

namespace Movies.Api.Common.Entities
{
    public class Movie
    {
        public Guid? ID { get; set; }

        [Required]
        public string Title { get; set; }

        public string Description { get; set; }

        [Required]
        public int RealeaseYear { get; set; }
    }
}
