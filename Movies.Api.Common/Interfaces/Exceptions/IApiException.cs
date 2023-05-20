using Movies.Api.Common.Errors.Enums;

namespace Movies.Api.Common.Interfaces.Exceptions
{
    public interface IApiException
    {
        ErrorCodes ErrorCode { get; }
        string Message { get; }
    }
}
