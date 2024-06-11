
namespace E_Commerce.Apis.Errors
{
    public class ApiExceptionResponse
    {

        public int StatusCode { get; set; }

        public string? ErrorMessage { get; set; }


        public ApiExceptionResponse(int statusCode, string? message=null)
        {
            StatusCode = statusCode;
            ErrorMessage = message??GetDefaultMessageForStatusCode(statusCode);
        }

        private string? GetDefaultMessageForStatusCode(int statusCode)
        {
            return statusCode switch
            {
             400=>"Bad Request",
             401=>"Un Authorized",
             404=>"Not Found",
             500=>"Internal Server Error",
             _ => null
            };
        }
    }
}
