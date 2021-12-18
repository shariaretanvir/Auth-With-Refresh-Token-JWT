using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AuthApp.Filters
{
    public class ModelValidationFilterAttribute : Attribute, IAsyncActionFilter
    {
        public ILogger<ModelValidationFilterAttribute> logger { get; }
        public ModelValidationFilterAttribute(ILogger<ModelValidationFilterAttribute> logger)
        {
            this.logger = logger;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!context.ModelState.IsValid)
            {
                context.Result = new BadRequestObjectResult(new
                {
                    Reason = "Validation Error",
                    ErrorMessage = context.ModelState.Values.Where(E => E.Errors.Count > 0)
                         .SelectMany(E => E.Errors)
                         .Select(E => E.ErrorMessage)
                         .ToList()
                });
                return;
            }

            await next.Invoke();
        }
    }
}
