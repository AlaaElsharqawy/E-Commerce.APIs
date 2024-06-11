using E_Commerce.Apis.Errors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.Apis.Controllers
{
    [Route("errors/{StatusCode}")]
    [ApiController]
    [ApiExplorerSettings(IgnoreApi =true)]
    public class ErrorsController : ControllerBase
    {
        public ActionResult Error(int StatusCode)
        {


            return NotFound(new ApiExceptionResponse(StatusCode));
        }

    }
}
