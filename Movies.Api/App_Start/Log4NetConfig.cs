using apache.log4net.Extensions.Logging;
using log4net;
using Microsoft.Extensions.Logging;
using Movies.Api.Common.Constants;
using System.IO;
using System.Web.Hosting;
using System.Web.Http;

namespace Movies.Api
{
    public static class Log4NetConfig
    {
        public static void Configure()
        {
            GlobalContext.Properties["RootDirectory"] = new DirectoryInfo(HostingEnvironment.ApplicationPhysicalPath).FullName;
            GlobalContext.Properties["ServiceName"] = Globals.MoviesServiceName;

            (GlobalConfiguration.Configuration.DependencyResolver.GetService(typeof(ILoggerFactory)) as LoggerFactory).AddLog4Net(new Log4NetSettings());
        }
    }
}
