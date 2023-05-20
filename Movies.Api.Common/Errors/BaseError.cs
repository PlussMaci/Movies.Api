namespace Movies.Api.Common.Errors
{
    public class BaseError
    {
        public int ErrorCode { get; set; }
        public string Message { get; set; }
    }
}
