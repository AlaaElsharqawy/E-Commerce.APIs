using E_Commerce.Apis.Errors;
using System.Net;
using System.Text.Json;

namespace E_Commerce.Apis.MiddleWares
{
    public class ExceptionMiddleWare
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleWare> _logger;
        private readonly IHostEnvironment _env;

        public ExceptionMiddleWare(RequestDelegate Next,ILogger<ExceptionMiddleWare>logger,IHostEnvironment env)
        {
            _next = Next;
           _logger = logger;
           _env = env;
        }



        //InvokeAsync

        public async Task InvokeAsync(HttpContext request)
        {
            try
            {
                await _next.Invoke(request);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex,ex.Message);
                //1
               //request.Response.ContentType = "application/json";
                //2
               request.Response.StatusCode=(int)HttpStatusCode.InternalServerError;

                var Response = _env.IsDevelopment() ? (new ApiInnerExceptionResponse((int)HttpStatusCode.InternalServerError,ex.Message,ex.StackTrace)) 
                    :( new ApiInnerExceptionResponse((int)HttpStatusCode.InternalServerError));
                //3
                //var jsonResponse = JsonSerializer.Serialize(Response, new JsonSerializerOptions
                //{
                //    PropertyNamingPolicy = JsonNamingPolicy.CamelCase

                //});
                //4
                //await request.Response.WriteAsync(jsonResponse);


                //instead of 2,3,4 
                await request.Response.WriteAsJsonAsync(Response);

            }           




        }


    }
}
