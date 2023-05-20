using Movies.Api.Common.Errors.Enums;
using Movies.Api.Common.Interfaces.Exceptions;
using System;

namespace Movies.Api.Common.Exceptions
{
    public class EntityAlreadyExistsException : InvalidOperationException, IApiException
    {
        public ErrorCodes ErrorCode => ErrorCodes.Duplicate;

        public EntityAlreadyExistsException(string message) : base(message)
        {

        }
    }
}
