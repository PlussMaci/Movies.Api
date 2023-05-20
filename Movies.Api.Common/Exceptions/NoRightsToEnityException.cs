using Movies.Api.Common.Errors.Enums;
using Movies.Api.Common.Interfaces.Exceptions;
using System;

namespace Movies.Api.Common.Exceptions
{
    public class NoRightsToEnityException : UnauthorizedAccessException, IApiException
    {
        public ErrorCodes ErrorCode => ErrorCodes.UnAuthorised;

        public NoRightsToEnityException(string message) : base(message)
        {

        }
    }
}
