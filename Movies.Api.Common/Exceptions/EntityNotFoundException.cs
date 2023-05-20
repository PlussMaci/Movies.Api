using Movies.Api.Common.Errors.Enums;
using Movies.Api.Common.Interfaces.Exceptions;
using System.Collections.Generic;

namespace Movies.Api.Common.Exceptions
{

    public class EntityNotFoundException : KeyNotFoundException, IApiException
    {
        public ErrorCodes ErrorCode => ErrorCodes.NoDataFound;

        public EntityNotFoundException(string message) : base(message)
        {

        }
    }
}
