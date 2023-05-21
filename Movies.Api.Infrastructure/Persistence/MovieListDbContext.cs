using Microsoft.Extensions.Logging;
using Movies.Api.Common.Entities;
using Movies.Api.Common.Errors.Enums;
using Movies.Api.Common.Exceptions;
using Movies.Api.Common.Extensions.Entities;
using Movies.Api.Common.Interfaces.DbContext;
using Movies.Api.Infrastructure.Result;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Movies.Api.Infrastructure.Persistence
{
    public class MovieListDbContext : IMovieListDbContext
    {
        private readonly ILogger<MovieListDbContext> _logger;

        public MovieListDbContext(ILogger<MovieListDbContext> logger)
        {
            _logger = logger;
        }

        private static ConcurrentDictionary<Guid, MovieList> _collection = new ConcurrentDictionary<Guid, MovieList>();

        public IDbResult<MovieList> Create(Guid CurrentUserID, MovieList model)
        {
            model.ID = Guid.NewGuid();
            model.UserID = CurrentUserID;

            model.CheckMovieIDs();

            if (_collection.TryAdd(model.ID, model))
            {
                return new DbResult<MovieList>( model);
            }

            return new DbResult<MovieList>(ErrorCodes.Duplicate, $"MovieList {model.ID} exists");
        }

        public IDbResult<bool> Delete(Guid CurrentUserID, Guid ID)
        {
            if (!_collection.TryGetValue(ID, out MovieList list))
            {
                return new DbResult<bool>(ErrorCodes.NoDataFound, $"MovieList {ID} not found");
            }

            if (list.UserID != CurrentUserID)
            {
                return new DbResult<bool>(ErrorCodes.UnAuthorised, $"The movie {ID} has other owner");
            }

            return new DbResult<bool>(_collection.TryRemove(ID, out _));
        }

        public IDbResult<MovieList> Edit(Guid CurrentUserID, MovieList model)
        {
            if (!_collection.TryGetValue(model.ID, out MovieList list))
            {
                return new DbResult<MovieList>(ErrorCodes.NoDataFound, $"MovieList {model.ID} not found");
            }

            if (list.UserID != CurrentUserID)
            {
                return new DbResult<MovieList>(ErrorCodes.UnAuthorised, $"The movie {model.ID} has other owner");
            }

            list.Merge(model);
            return new DbResult<MovieList>(list);
        }

        public IDbResult<IEnumerable<MovieList>> GetAll(Guid CurrentUserID)
        {
            return new DbResult<IEnumerable<MovieList>>(_collection.Values);
        }

        public IDbResult<MovieList> GetByID(Guid CurrentUserID, Guid ID)
        {
            if (_collection.TryGetValue(ID, out MovieList entity))
            {
                return new DbResult<MovieList>(entity);
            }

            return new DbResult<MovieList>(ErrorCodes.NoDataFound, $"MovieList {ID} not found");
        }

        public IDbResult<IEnumerable<MovieList>> SearchByName(Guid CurrentUserID, string name)
        {
            return new DbResult<IEnumerable<MovieList>>(_collection.Values.Where(x => x.Title.ToLowerInvariant().Contains(name.ToLowerInvariant())));
        }
    }
}
