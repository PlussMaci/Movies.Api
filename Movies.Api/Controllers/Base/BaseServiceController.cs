using Microsoft.Extensions.Logging;
using Movies.Api.Common.Constants;
using Movies.Api.Common.Errors;
using Movies.Api.Common.Errors.Enums;
using Movies.Api.Common.Interfaces.DbContext;
using Movies.Api.Common.Results;
using System;
using System.Net;
using System.Web.Http;
using Movies.Api.Common.Extensions.Errors;

namespace Movies.Api.Controllers.Base
{
    public class BaseServiceController<TEntity> : ApiController
    {
        protected readonly IBaseDbContext<TEntity> _dbContext;
        protected readonly ILogger _logger;

        public BaseServiceController(IBaseDbContext<TEntity> dbContext, ILogger<TEntity> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        protected CustomResult<T> OtherResult<T>(T data) where T : BaseError, new()
        {
            return new CustomResult<T>(Request, ((ErrorCodes)data.ErrorCode).MapErrorCode(), data);
        }

        protected CustomResult<T> OtherException<T>(Exception e, ErrorCodes ErrorCode = ErrorCodes.Unknown) where T : BaseError, new()
        {
            return OtherException<T>(e, ErrorCode);
        }

        protected CustomResult<T> OtherException<T>(string Message, ErrorCodes ErrorCode, HttpStatusCode statusCode = HttpStatusCode.InternalServerError) where T : BaseError, new()
        {
            return new CustomResult<T>(Request, statusCode, new T()
            {
                ErrorCode = (int)ErrorCode,
                Message = string.Format("{0}: {1}", Globals.MoviesServiceName, Message)
            });
        }
    }
}