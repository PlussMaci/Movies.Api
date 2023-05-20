using Microsoft.Extensions.Logging;
using Microsoft.Web.Http;
using Movies.Api.Common.Attributes;
using Movies.Api.Common.Entities;
using Movies.Api.Common.Interfaces.DbContext;
using Movies.Api.Controllers.Base;
using System;
using System.Web.Http;
using Movies.Api.Common.Tokens;
using Movies.Api.Models;
using System.Collections.Generic;
using Movies.Api.Common.Errors;
using Movies.Api.Common.Errors.Enums;
using System.Linq;

namespace Movies.Api.Controllers.V1
{
    [ApiVersion("1.0")]
    [LogApi(typeof(ILogger<User>))]
    [ApiExceptionFilter(typeof(ILogger<User>))]
    [RoutePrefix("api/v{version:apiVersion}/account")]
    public class AccountController : BaseServiceController<User>
    {
        public AccountController(IUserDbContext dbContext, ILogger<User> logger) : base(dbContext, logger)
        {

        }

        [HttpPost]
        [Route("")]
        public IHttpActionResult Login(AccountModel account)
        {
            var loggedInUser = _dbContext.SearchByName(Guid.Empty, account.UserName);

            User currentUser = loggedInUser.Result?.FirstOrDefault();

            if (loggedInUser.Result is null || loggedInUser.Result.Count() == 0)
            {
                var newUser = _dbContext.Create(Guid.Empty, new User()
                {
                    Name = account.UserName,
                    Password = account.Password
                });

                if (!newUser.IsOk)
                {
                    return OtherResult(newUser.ToBaseError());
                }

                currentUser = newUser.Result;
            }
            else if (currentUser.Password != account.Password)
            {
                return OtherResult(new BaseError()
                {
                    ErrorCode = (int)ErrorCodes.UnAuthorised,
                    Message = "Wrong username or password!"
                });
            }

            var roles = new List<string>() { "user" };

            if (currentUser.Name == "admin")
            {
                roles.Add("admin");
            }

            var token = TokenHelper.GetToken(currentUser, roles, out DateTime expires);

            return Ok(new LoginResult() { token = token, expires = expires });
        }
    }
}