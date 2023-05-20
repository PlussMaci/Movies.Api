using Microsoft.Extensions.Logging;
using Microsoft.Web.Http;
using Movies.Api.Common.Attributes;
using Movies.Api.Common.Entities;
using Movies.Api.Common.Helpers;
using Movies.Api.Common.Interfaces.DbContext;
using Movies.Api.Controllers.Base;
using System;
using System.Web.Http;

namespace Movies.Api.Controllers.V1
{
    [ApiVersion("1.0")]
    [LogApi(typeof(ILogger<Movie>))]
    [ApiExceptionFilter(typeof(ILogger<Movie>))]
    [RoutePrefix("api/v{version:apiVersion}/movieLists")]
    public class MovieListController : BaseServiceController<MovieList>
    {
        public MovieListController(IMovieListDbContext dbContext, ILogger<MovieList> logger) : base(dbContext, logger)
        {

        }

        [HttpGet]
        [Route("")]
        [AllowAnonymous]
        public IHttpActionResult GetAll()
        {
            _logger.LogDebug("GetAll called");
            var result = _dbContext.GetAll(UserIDHelper.GetUserID(User));

            if (result.IsOk)
            {
                return Ok(result.Result);
            }

            return OtherResult(result.ToBaseError());
        }

        [HttpGet]
        [Route("{ID:guid}")]
        public IHttpActionResult GetByID(Guid ID)
        {
            var result = _dbContext.GetByID(UserIDHelper.GetUserID(User), ID);

            if (result.IsOk)
            {
                return Ok(result.Result);
            }

            return OtherResult(result.ToBaseError());
        }

        [HttpPost]
        [Route("")]
        [Authorize(Roles = "admin,user")]
        public IHttpActionResult Create(MovieList model)
        {
            var result = _dbContext.Create(UserIDHelper.GetUserID(User), model);

            if (result.IsOk)
            {
                return Ok(result.Result);
            }

            return OtherResult(result.ToBaseError());
        }

        [HttpPut]
        [Route("")]
        [Authorize(Roles = "admin,user")]
        public IHttpActionResult Edit(MovieList model)
        {
            var result = _dbContext.Edit(UserIDHelper.GetUserID(User), model);

            if (result.IsOk)
            {
                return Ok(result.Result);
            }

            return OtherResult(result.ToBaseError());
        }

        [HttpDelete]
        [Route("{ID:guid}")]
        [Authorize(Roles = "admin")]
        public IHttpActionResult Delete(Guid ID)
        {
            var result = _dbContext.Delete(UserIDHelper.GetUserID(User), ID);

            if (result.IsOk)
            {
                return Ok();
            }

            return OtherResult(result.ToBaseError());

        }

        [HttpGet]
        [Route("movie/{name}")]
        public IHttpActionResult Search(string name)
        {
            var result = _dbContext.SearchByName(UserIDHelper.GetUserID(User), name);

            if (result.IsOk)
            {
                return Ok(result.Result);
            }

            return OtherResult(result.ToBaseError());

        }
    }
}