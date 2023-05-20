using Movies.Api.Common.Errors.Enums;
using System.Net;

namespace Movies.Api.Common.Extensions.Errors
{
    public static class ErrorExtensions
    {
        public static HttpStatusCode MapErrorCode(this ErrorCodes errorCode)
        {
            switch (errorCode)
            {
                case ErrorCodes.NoError:
                    return HttpStatusCode.OK;
                case ErrorCodes.Duplicate:
                    return HttpStatusCode.Conflict;
                case ErrorCodes.NoDataFound:
                    return HttpStatusCode.NotFound;
                case ErrorCodes.UnAuthorised:
                    return HttpStatusCode.Unauthorized;
                case ErrorCodes.Unknown:
                default:
                    return HttpStatusCode.InternalServerError;

            }
        }
    }
}
