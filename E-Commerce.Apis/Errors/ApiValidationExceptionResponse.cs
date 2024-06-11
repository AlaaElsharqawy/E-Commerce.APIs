namespace E_Commerce.Apis.Errors
{
    public class ApiValidationExceptionResponse:ApiExceptionResponse
    {

        public IEnumerable<string> Errors { get; set; }//refrence

        public ApiValidationExceptionResponse():base(400)
        {
           Errors = new List<string>();
        }



    }
}
