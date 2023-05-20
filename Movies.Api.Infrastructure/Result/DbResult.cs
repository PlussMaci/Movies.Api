using Movies.Api.Common.Errors;
using Movies.Api.Common.Errors.Enums;
using Movies.Api.Common.Interfaces.DbContext;

namespace Movies.Api.Infrastructure.Result
{
    public class DbResult<T>: IDbResult<T>
    {
        public DbResult(T data)
        {
            Result = data;
            ErrorCode = ErrorCodes.NoError;
        }

        public DbResult(ErrorCodes errorCode, string errorMessage)
        {
            ErrorCode = errorCode;
            ErrorMessage = errorMessage;
        }

        public T Result { get; set; }
        public ErrorCodes ErrorCode { get; set; }
        public string ErrorMessage { get; set; }
        public bool IsOk => ErrorCode == ErrorCodes.NoError;

        public BaseError ToBaseError()
        {
            return new BaseError()
            {
                ErrorCode = (int)this.ErrorCode,
                Message = this.ErrorMessage
            };
        }
    }
}
