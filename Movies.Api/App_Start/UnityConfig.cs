using Microsoft.Extensions.Logging;
using Movies.Api.Common.Interfaces.DbContext;
using Movies.Api.Infrastructure.Persistence;
using System;

using Unity;

namespace Movies.Api
{
    /// <summary>
    /// Specifies the Unity configuration for the main container.
    /// </summary>
    public static class UnityConfig
    {
        #region Unity Container
        private static Lazy<IUnityContainer> container =
          new Lazy<IUnityContainer>(() =>
          {
              var container = new UnityContainer();
              RegisterTypes(container);
              return container;
          });

        /// <summary>
        /// Configured Unity Container.
        /// </summary>
        public static IUnityContainer Container => container.Value;
        #endregion

        /// <summary>
        /// Registers the type mappings with the Unity container.
        /// </summary>
        /// <param name="container">The unity container to configure.</param>
        /// <remarks>
        /// There is no need to register concrete types such as controllers or
        /// API controllers (unless you want to change the defaults), as Unity
        /// allows resolving a concrete type even if it was not previously
        /// registered.
        /// </remarks>
        public static void RegisterTypes(IUnityContainer container)
        {
            container.RegisterSingleton<IMovieListDbContext, MovieListDbContext>();
            container.RegisterSingleton<IUserDbContext, UserDbContext>();
            container.RegisterSingleton<ILoggerFactory, LoggerFactory>();
            container.RegisterType(typeof(ILogger<>), typeof(Logger<>));
        }
    }
}