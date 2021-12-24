using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AuthApp.Filters
{
    public class CacheFilterAttribute : IAsyncActionFilter
    {
        private readonly ILogger<CacheFilterAttribute> logger;
        private readonly IMemoryCache memoryCache;

        public CacheFilterAttribute(ILogger<CacheFilterAttribute> logger,IMemoryCache memoryCache)
        {
            this.logger = logger;
            this.memoryCache = memoryCache;
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var queryString = context.HttpContext.Request.QueryString;
            var cacheKey = "weathers-"+ queryString.ToString();
            if(memoryCache.TryGetValue(cacheKey, out List<WeatherForecast> weathers))
            {
                context.Result = new OkObjectResult(weathers);
            }
            else
            {
                await next.Invoke();
            }
        }
    }
}
