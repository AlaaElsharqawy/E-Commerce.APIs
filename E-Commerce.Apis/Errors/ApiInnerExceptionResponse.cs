namespace E_Commerce.Apis.Errors
{
    public class ApiInnerExceptionResponse:ApiExceptionResponse
    {

        public string? Details { get; set; }

        public ApiInnerExceptionResponse(int statusCode,string errorMessage=null, string details=null):base(statusCode,errorMessage )
        {
            Details = details;
        }


    }
}
