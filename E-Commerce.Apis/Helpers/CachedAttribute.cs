using E_Commerce.Core.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Text;

namespace E_Commerce.Apis.Helpers
{
    public class CachedAttribute : Attribute, IAsyncActionFilter
    {
        private readonly int _timeToLiveSeconds;

        public CachedAttribute(int timeToLiveSeconds) 
        {
            this._timeToLiveSeconds = timeToLiveSeconds;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
          var responseCacheService=context.HttpContext.RequestServices.GetRequiredService<IResponseCacheService>();
            //Ask Clr for creating object from "ResponseCacheService" Explicity

            var cacheKey = GenerateCacheKeyFromRequest(context.HttpContext.Request);
            var response = await responseCacheService.GetCachedResponseAsync(cacheKey);
            if(!string.IsNullOrEmpty(response))
            {
                var result = new ContentResult() 
                {
                    Content=response,
                    ContentType="application/json"  ,
                    StatusCode=200
                
                };
                context.Result = result;
                return;

            }


            var executedActionContext = await next.Invoke();
            if(executedActionContext.Result is OkObjectResult okObjectResult&&okObjectResult.Value is not null)
            {
                await responseCacheService.CacheResponseAsync(cacheKey,okObjectResult.Value,TimeSpan.FromSeconds(_timeToLiveSeconds));
            }
        }

        private string GenerateCacheKeyFromRequest(HttpRequest request)
        {
            //urlBase/api/products?pageIndex=1&pageSize=5&sort=name

            var KeyBuilder=new StringBuilder();
            KeyBuilder.Append(request.Path);

            //  pageIndex=1
            //  pageSize=5
            // sort=name
            foreach(var (key, value) in request.Query.OrderBy(x=>x.Key)) 
            {
                KeyBuilder.Append($"|{key}-{value}");
                //api/products| pageIndex-1
                //api/products| pageIndex-1| pageSize-5
                //api/products| pageIndex-1| pageSize-5|sort-name
            }
            return KeyBuilder.ToString();

        }
    }
}
            