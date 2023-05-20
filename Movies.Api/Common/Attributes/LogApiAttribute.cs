using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Web.Http;
using Movies.Api.Common.Handlers;

namespace Movies.Api.Common.Attributes
{
    public class LogApiAttribute : ActionFilterAttribute
    {
        public override object TypeId { get { return new object(); } }
        private readonly ILogger _logger;

        public LogApiAttribute(Type loggerType)
        {
            _logger = GlobalConfiguration.Configuration.DependencyResolver.GetService(loggerType) as ILogger;
        }

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            if (_logger.IsEnabled(LogLevel.Debug))
            {
                _logger.LogDebug("Action: {0}", actionContext.ActionDescriptor.ActionName);
                _logger.LogDebug("Request URL: {0}", actionContext.Request.RequestUri);
                _logger.LogDebug("Raw request: {0}", actionContext.Request.Properties[SaveRawPostDataHandler.RawDataKey]);
                _logger.LogDebug("Binded params: {0}", JsonConvert.SerializeObject(actionContext.ActionArguments));
            }

            base.OnActionExecuting(actionContext);
        }

        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            base.OnActionExecuted(actionExecutedContext);

            if (actionExecutedContext.Exception is null && _logger.IsEnabled(LogLevel.Debug))
            {
                _logger.LogDebug("{0} Result: {1}", actionExecutedContext.ActionContext.ActionDescriptor.ActionName, JsonConvert.SerializeObject((actionExecutedContext.Response.Content as System.Net.Http.ObjectContent)?.Value));
            }
            else if (actionExecutedContext.Exception != null)
            {
                _logger.LogError(actionExecutedContext.Exception, "{0} Exception", actionExecutedContext.ActionContext.ActionDescriptor.ActionName);
            }
        }
    }
}