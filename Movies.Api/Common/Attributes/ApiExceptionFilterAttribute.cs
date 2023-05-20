using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Web.Http.Filters;
using System.Web.Http;
using Movies.Api.Common.Errors.Enums;
using Movies.Api.Common.Errors;
using Movies.Api.Common.Interfaces.Exceptions;
using Movies.Api.Common.Results;
using System.Net;
using Movies.Api.Common.Extensions.Errors;

namespace Movies.Api.Common.Attributes
{
    public class ApiExceptionFilterAttribute : ExceptionFilterAttribute
    {
        public override object TypeId { get { return new object(); } }
        private readonly ILogger _logger;

        public ApiExceptionFilterAttribute(Type loggerType)
        {
            _logger = GlobalConfiguration.Configuration.DependencyResolver.GetService(loggerType) as ILogger;
        }

        public override void OnException(HttpActionExecutedContext context)
        {
            if (context.Exception is IApiException)
            {
                var ex = (IApiException)context.Exception;

                context.Response = new CustomResult<BaseError>(context.Request, ex.ErrorCode.MapErrorCode(), new BaseError()
                {
                    ErrorCode = (int)ex.ErrorCode,
                    Message = ex.Message
                }).Execute();
            }
            else
            {
                context.Response = new CustomResult<BaseError>(context.Request, HttpStatusCode.InternalServerError, new BaseError()
                {
                    ErrorCode = (int)ErrorCodes.Unknown,
                    Message = context.Exception.Message
                }).Execute();

                _logger.LogError("{0} Result: {1}", context.ActionContext.ActionDescriptor.ActionName, JsonConvert.SerializeObject((context.Response.Content as System.Net.Http.ObjectContent).Value));
            }
        }
    }
}