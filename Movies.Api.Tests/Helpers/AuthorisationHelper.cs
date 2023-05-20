using Moq;
using System;
using System.Collections.ObjectModel;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace Movies.Api.Tests.Helpers
{
    internal static class AuthorisationHelper
    {
        internal static bool CallAuthorization<T>(ApiController controller, Func<T, IHttpActionResult> method)
        {
            var descriptor = new Mock<HttpActionDescriptor>();
            descriptor.Setup(x => x.GetCustomAttributes<AllowAnonymousAttribute>()).Returns(new Collection<AllowAnonymousAttribute>());

            controller.ActionContext.ActionDescriptor = descriptor.Object;
            controller.Request = new HttpRequestMessage();

            foreach (AuthorizeAttribute item in method.Method.GetCustomAttributes(typeof(AuthorizeAttribute), true))
            {
                item.OnAuthorization(controller.ActionContext);

                var response = controller.ActionContext.Response;

                if (response != null && response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
