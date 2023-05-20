using Microsoft.Owin.Security.OAuth;
using Microsoft.Web.Http.Routing;
using Movies.Api.Common.Attributes;
using Movies.Api.Common.Handlers;
using System.Web.Http;
using System.Web.Http.Routing;

namespace Movies.Api
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.SuppressDefaultHostAuthentication();
            config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));
            config.Filters.Add(new ValidateModelAttribute());

            var constraintResolver = new DefaultInlineConstraintResolver()
            {
                ConstraintMap =
                {
                    ["apiVersion"] = typeof( ApiVersionRouteConstraint )
                }
            };

            config.MapHttpAttributeRoutes(constraintResolver);
            config.AddApiVersioning(o => {
                o.AssumeDefaultVersionWhenUnspecified = true;
                o.DefaultApiVersion = new Microsoft.Web.Http.ApiVersion(1, 0);
            });

            config.MessageHandlers.Add(new SaveRawPostDataHandler());

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                 routeTemplate: "api/v{apiVersion}/{controller}/{action}/{id}",
                 defaults: new { id = RouteParameter.Optional },
                 constraints: new { apiVersion = new ApiVersionRouteConstraint() }
            );
        }
    }
}
