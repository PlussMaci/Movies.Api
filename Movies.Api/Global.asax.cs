using Microsoft.Extensions.Logging;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Movies.Api
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            Log4NetConfig.Configure();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            (GlobalConfiguration.Configuration.DependencyResolver.GetService(typeof(ILogger<WebApiApplication>)) as ILogger<WebApiApplication>).LogDebug("Service starting...");
        }
    }
}
