namespace Movies.Api.Common.Errors.Enums
{
    public enum ErrorCodes
    {
        NoError = 0,
        Duplicate = -1,
        NoDataFound = -2,
        UnAuthorised = -3,
        Unknown = -9999
    }
}
