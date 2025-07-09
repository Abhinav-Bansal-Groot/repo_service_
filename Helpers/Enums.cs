using System.ComponentModel;

namespace repo_service.Helpers
{
    public enum ResponseCodes
    {
        [Description("Error")]
        Error = 0,
        [Description("Success")]
        Success = 200,
        [Description("Invalid Request")]
        InvalidRequest = 302,
        [Description("InvalidUser")]
        InvalidUser = 301,
        [Description("Duplicate")]
        Duplicate = 300,
        [Description("Invalid Model")]
        InvalidModel = 303,
        [Description("UnAuthorized User")]
        Unauthorized = 401,
        [Description("Not Found")]
        NotFound = 404,
        [Description("Bad Request")]
        ErrorCode = 400,
        [Description("Internal Server Error")]
        InternalError = 500
    }
}
