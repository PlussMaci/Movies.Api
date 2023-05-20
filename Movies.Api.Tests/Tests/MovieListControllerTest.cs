using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Movies.Api.Common.Entities;
using Movies.Api.Common.Interfaces.DbContext;
using Movies.Api.Controllers.V1;
using Movies.Api.Infrastructure.Persistence;
using NUnit.Framework;
using System.Web.Http.Results;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using Movies.Api.Common.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Movies.Api.Common.Results;
using Movies.Api.Common.Errors;
using System.Net;
using Movies.Api.Common.Errors.Enums;
using Movies.Api.Tests.Helpers;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Net.Http;

namespace Movies.Api.Tests.Tests
{
    [TestFixture]
    public class MovieListControllerTest
    {
        private ServiceProvider _serviceProvider;
        private MovieListController _controller;

        [OneTimeSetUp]
        public void Init()
        {
            _serviceProvider = new ServiceCollection()
               .AddLogging()
               .AddSingleton<IMovieListDbContext, MovieListDbContext>()
               .BuildServiceProvider();
        }

        [SetUp]
        public void TestInit()
        {
            _controller = new MovieListController(
                                _serviceProvider.GetService<IMovieListDbContext>(),
                                _serviceProvider.GetService<ILoggerFactory>().CreateLogger<MovieList>());

            _controller.ControllerContext.ControllerDescriptor = new HttpControllerDescriptor(new HttpConfiguration(), "MovieListController", typeof(MovieListController));
        }

        [Test]
        public void GetAll()
        {
            var result = _controller.GetAll() as OkNegotiatedContentResult<IEnumerable<MovieList>>;

            Assert.IsNotNull(result, "Result is null");
        }

        [Test]
        public void GetByID()
        {
            UserHelper.SetUser(out var userID, _controller, false);
            Assert.IsTrue(AuthorisationHelper.CallAuthorization<MovieList>(_controller, _controller.Create), "Auth failed");

            var resultCreate = _controller.Create(new MovieList()
            {
                Title = "Test",
                Description = "Test desc",
                movies = new List<Movie> {
                    new Movie {
                        Title = "Test movie",
                        Description = "Test desc",
                        RealeaseYear = 2023
                    }
            }
            }) as OkNegotiatedContentResult<MovieList>;

            Assert.IsTrue(AuthorisationHelper.CallAuthorization<Guid>(_controller, _controller.GetByID), "Auth failed");
            var result = _controller.GetByID(resultCreate.Content.ID) as OkNegotiatedContentResult<MovieList>;

            Assert.IsNotNull(result, "Result is null");
            Assert.AreEqual(result.Content.ID, resultCreate.Content.ID, "IDs are not the same");
        }

        [Test]
        public void GetByIDNotFound()
        {
            UserHelper.SetUser(out var userID, _controller, false);
            Assert.IsTrue(AuthorisationHelper.CallAuthorization<Guid>(_controller, _controller.GetByID), "Auth failed");

            var result = _controller.GetByID(Guid.Parse("872215ee-fd75-4ede-9ebf-9b030aa6f013")) as CustomResult<BaseError>;

            Assert.IsNotNull(result, "Result is not null");
            Assert.IsTrue(result.statusCode == HttpStatusCode.NotFound, "Wrong status code");
            Assert.IsTrue(result.data.ErrorCode == (int)ErrorCodes.NoDataFound, "Wrong error code");
        }

        [Test]
        public void Create()
        {
            UserHelper.SetUser(out var userID, _controller, false);
            Assert.IsTrue(AuthorisationHelper.CallAuthorization<MovieList>(_controller, _controller.Create), "Auth failed");

            var result = _controller.Create(new MovieList()
            {
                Title = "Test",
                Description = "Test desc",
                movies = new List<Movie> {
                    new Movie {
                        Title = "Test movie",
                        Description = "Test desc",
                        RealeaseYear = 2023
                    }
            }
            }) as OkNegotiatedContentResult<MovieList>;

            Assert.IsTrue(result.Content.movies.Count == 1, "Wrong count");
            Assert.IsTrue(result.Content.movies[0].RealeaseYear == 2023, "Wrong year");
            Assert.AreEqual(result.Content.UserID, userID, "Wrong user");
            Assert.AreNotEqual(result.Content.ID, Guid.Empty, "Wrong ID");
        }

        [Test]
        public void Edit()
        {
            UserHelper.SetUser(out var userID, _controller, false);
            Assert.IsTrue(AuthorisationHelper.CallAuthorization<MovieList>(_controller, _controller.Create), "Auth failed");

            var resultCreate = _controller.Create(new MovieList()
            {
                Title = "Test",
                Description = "Test desc",
                movies = new List<Movie> {
                    new Movie {
                        Title = "Test movie",
                        Description = "Test desc",
                        RealeaseYear = 2023
                    }
            }
            }) as OkNegotiatedContentResult<MovieList>;

            resultCreate.Content.Title = "Test1";
            resultCreate.Content.movies[0].RealeaseYear = 2022;

            Assert.IsTrue(AuthorisationHelper.CallAuthorization<MovieList>(_controller, _controller.Edit), "Auth failed");
            var result = _controller.Edit(resultCreate.Content) as OkNegotiatedContentResult<MovieList>;

            Assert.IsTrue(result.Content.Title == resultCreate.Content.Title, "Wrong title");
            Assert.IsTrue(result.Content.movies.Count == resultCreate.Content.movies.Count, "Wrong count");
            Assert.IsTrue(result.Content.movies[0].RealeaseYear == resultCreate.Content.movies[0].RealeaseYear, "Wrong year");
            Assert.AreEqual(result.Content.UserID, userID, "Wrong user");
            Assert.AreNotEqual(result.Content.ID, Guid.Empty, "Wrong ID");
        }

        [Test]
        public void EditDifferentUser()
        {
            UserHelper.SetUser(out var userID, _controller, false);
            Assert.IsTrue(AuthorisationHelper.CallAuthorization<MovieList>(_controller, _controller.Create), "Auth failed");
            var resultCreate = _controller.Create(new MovieList()
            {
                Title = "Test",
                Description = "Test desc",
                movies = new List<Movie> {
                    new Movie {
                        Title = "Test movie",
                        Description = "Test desc",
                        RealeaseYear = 2023
                    }
            }
            }) as OkNegotiatedContentResult<MovieList>;

            resultCreate.Content.Title = "Test1";
            resultCreate.Content.movies[0].RealeaseYear = 2022;

            UserHelper.SetUser(out userID, _controller, false);
            Assert.IsTrue(AuthorisationHelper.CallAuthorization<MovieList>(_controller, _controller.Edit), "Auth failed");

            var result = _controller.Edit(resultCreate.Content) as CustomResult<BaseError>;

            Assert.IsTrue(result.statusCode == HttpStatusCode.Unauthorized, "Authorized");
        }

        [Test]
        public void DeleteWithAdmin()
        {
            UserHelper.SetUser(out var userID, _controller, true);
            Assert.IsTrue(AuthorisationHelper.CallAuthorization<MovieList>(_controller, _controller.Create), "Auth failed");

            var resultCreate = _controller.Create(new MovieList()
            {
                Title = "Test",
                Description = "Test desc",
                movies = new List<Movie> {
                    new Movie {
                        Title = "Test movie",
                        Description = "Test desc",
                        RealeaseYear = 2023
                    }
            }
            }) as OkNegotiatedContentResult<MovieList>;

            Assert.IsTrue(AuthorisationHelper.CallAuthorization<Guid>(_controller, _controller.Delete), "Auth failed");
            var result = _controller.Delete(resultCreate.Content.ID) as OkResult;

            Assert.IsNotNull(result, "Result is null");
        }

        [Test]
        public void DeleteWithUser()
        {
            UserHelper.SetUser(out var userID, _controller, false);
            Assert.IsTrue(AuthorisationHelper.CallAuthorization<MovieList>(_controller, _controller.Create), "Auth failed");

            var resultCreate = _controller.Create(new MovieList()
            {
                Title = "Test",
                Description = "Test desc",
                movies = new List<Movie> {
                    new Movie {
                        Title = "Test movie",
                        Description = "Test desc",
                        RealeaseYear = 2023
                    }
            }
            }) as OkNegotiatedContentResult<MovieList>;

            Assert.IsFalse(AuthorisationHelper.CallAuthorization<Guid>(_controller, _controller.Delete), "Auth failed");
            var result = _controller.Delete(resultCreate.Content.ID) as OkResult;

            Assert.IsNotNull(result, "Result is null");
        }

        [Test]
        [TestCase("Dummy movie")]
        public void SearchByName(string movieName)
        {
            UserHelper.SetUser(out var userID, _controller, false);
            Assert.IsTrue(AuthorisationHelper.CallAuthorization<MovieList>(_controller, _controller.Create), "Auth failed");

            var resultCreate = _controller.Create(new MovieList()
            {
                Title = movieName,
                Description = "Test desc",
                movies = new List<Movie> {
                    new Movie {
                        Title = "Test movie",
                        Description = "Test desc",
                        RealeaseYear = 2023
                    }
            }
            }) as OkNegotiatedContentResult<MovieList>;

            var result = _controller.Search(movieName) as OkNegotiatedContentResult<IEnumerable<MovieList>>;

            Assert.IsNotNull(result, "Result is null");
        }
    }
}
