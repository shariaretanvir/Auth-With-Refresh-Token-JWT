using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace AuthApp.Filters
{
    public class CommonRequestFilter : IAsyncActionFilter
    {
        public ILogger<CommonRequestFilter> _Logger { get; }
        public CommonRequestFilter(ILogger<CommonRequestFilter> logger)
        {
            _Logger = logger;
        }


        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            //code before action executes
            var a = context.ActionArguments.ToString();
            _Logger.LogInformation(context.ActionArguments.ToString());
            var result = await next();
            //code acter action executes
            var response = context.ActionArguments.ToString();
        }
    }
}
