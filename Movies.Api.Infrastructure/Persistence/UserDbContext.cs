using Microsoft.Extensions.Logging;
using Movies.Api.Common.Entities;
using Movies.Api.Common.Errors.Enums;
using Movies.Api.Common.Interfaces.DbContext;
using Movies.Api.Infrastructure.Result;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Movies.Api.Infrastructure.Persistence
{
    public class UserDbContext : IUserDbContext
    {
        private readonly ILogger<UserDbContext> _logger;

        public UserDbContext(ILogger<UserDbContext> logger)
        {
            _logger = logger;
        }

        private static ConcurrentDictionary<Guid, User> _collection = new ConcurrentDictionary<Guid, User>();

        public IDbResult<User> Create(Guid CurrentUserID, User model)
        {
            model.ID = Guid.NewGuid();

            if (_collection.TryAdd(model.ID, model))
            {
                return new DbResult<User>(model);
            }

            return new DbResult<User>(ErrorCodes.Duplicate, $"User {model.ID} exists");
        }

        public IDbResult<bool> Delete(Guid CurrentUserID, Guid ID)
        {
            throw new NotImplementedException();
        }

        public IDbResult<User> Edit(Guid CurrentUserID, User model)
        {
            throw new NotImplementedException();
        }

        public IDbResult<IEnumerable<User>> GetAll(Guid CurrentUserID)
        {
            throw new NotImplementedException();
        }

        public IDbResult<User> GetByID(Guid CurrentUserID, Guid ID)
        {
            throw new NotImplementedException();
        }

        public IDbResult<IEnumerable<User>> SearchByName(Guid CurrentUserID, string name)
        {
            return new DbResult<IEnumerable<User>>(_collection.Values.Where(x => x.Name == name));
        }
    }
}
