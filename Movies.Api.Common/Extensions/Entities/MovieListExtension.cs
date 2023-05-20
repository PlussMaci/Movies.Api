using Movies.Api.Common.Entities;
using System;
using System.Linq;

namespace Movies.Api.Common.Extensions.Entities
{
    public static class MovieListExtension
    {
        public static MovieList Merge(this MovieList current, MovieList other)
        {
            current.Title = other.Title;
            current.Description = other.Description;
            current.movies = other.movies;

            return current.CheckMovieIDs();
        }

        public static MovieList CheckMovieIDs(this MovieList current)
        {
            if (current.movies != null)
            {
                foreach (var movie in current.movies.Where(x => x.ID is null))
                {
                    movie.ID = Guid.NewGuid();
                }
            }

            return current;
        }
    }
}
