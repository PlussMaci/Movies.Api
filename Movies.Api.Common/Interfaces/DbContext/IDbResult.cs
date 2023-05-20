using Movies.Api.Common.Errors;
using Movies.Api.Common.Errors.Enums;

namespace Movies.Api.Common.Interfaces.DbContext
{
    public interface IDbResult<T>
    {
        T Result { get; set; }
        ErrorCodes ErrorCode { get; set; }
        string ErrorMessage { get; set; }
        bool IsOk { get; }
        BaseError ToBaseError();
    }
}
