using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Movies.Api.Common.Entities;
using Movies.Api.Common.Errors;
using Movies.Api.Common.Errors.Enums;
using Movies.Api.Common.Interfaces.DbContext;
using Movies.Api.Common.Results;
using Movies.Api.Controllers.V1;
using Movies.Api.Infrastructure.Persistence;
using Movies.Api.Models;
using NUnit.Framework;
using System;
using System.Web.Http.Results;

namespace Movies.Api.Tests.Tests
{
    [TestFixture]
    public class AccountControllerTest
    {
        private ServiceProvider _serviceProvider;
        private AccountController _controller;

        [OneTimeSetUp]
        public void Init()
        {
            _serviceProvider = new ServiceCollection()
               .AddLogging()
               .AddSingleton<IUserDbContext, UserDbContext>()
               .BuildServiceProvider();

            _controller = new AccountController(
                                _serviceProvider.GetService<IUserDbContext>(),
                                _serviceProvider.GetService<ILoggerFactory>().CreateLogger<User>());
        }

        [Test]
        public void LoginAdmin()
        {
            var result = _controller.Login(new AccountModel()
            {
                Password = "admin",
                UserName = "admin"
            }) as OkNegotiatedContentResult<LoginResult>;

            Assert.Greater(result.Content.expires, DateTime.Now);
        }

        [Test]
        public void LoginUser()
        {
            var result = _controller.Login(new AccountModel()
            {
                Password = "password",
                UserName = "userName"
            }) as OkNegotiatedContentResult<LoginResult>;

            Assert.Greater(result.Content.expires, DateTime.Now);
        }

        [Test]
        public void LoginUserInvalid()
        {
            var result = _controller.Login(new AccountModel()
            {
                Password = "password",
                UserName = "userName"
            }) as OkNegotiatedContentResult<LoginResult>;

            Assert.Greater(result.Content.expires, DateTime.Now);

            var resultNOK = _controller.Login(new AccountModel()
            {
                Password = "password1",
                UserName = "userName"
            }) as CustomResult<BaseError>;

            Assert.True(resultNOK.statusCode == System.Net.HttpStatusCode.Unauthorized);
            Assert.True(resultNOK.data.ErrorCode == (int)ErrorCodes.UnAuthorised);
        }
    }
}
