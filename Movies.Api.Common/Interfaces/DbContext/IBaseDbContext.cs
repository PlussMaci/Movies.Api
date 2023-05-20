using System;
using System.Collections.Generic;

namespace Movies.Api.Common.Interfaces.DbContext
{
    public interface IBaseDbContext<T>
    {
        IDbResult<IEnumerable<T>> GetAll(Guid CurrentUserID);
        IDbResult<T> GetByID(Guid CurrentUserID, Guid ID);
        IDbResult<T> Create(Guid CurrentUserID, T model);
        IDbResult<T> Edit(Guid CurrentUserID, T model);
        IDbResult<bool> Delete(Guid CurrentUserID, Guid ID);
        IDbResult<IEnumerable<T>> SearchByName(Guid CurrentUserID, string name);
    }
}
