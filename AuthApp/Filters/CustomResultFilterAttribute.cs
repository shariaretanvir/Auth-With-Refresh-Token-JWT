using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;

namespace AuthApp.Filters
{
    public class CustomResultFilterAttribute : Attribute, IResultFilter
    {
        public ILogger<CustomResultFilterAttribute> Logger { get; }
        public CustomResultFilterAttribute(ILogger<CustomResultFilterAttribute> logger)
        {
            Logger = logger;
        }

        public void OnResultExecuted(ResultExecutedContext context)
        {
            Logger.LogInformation("Before Result filter");
        }

        public void OnResultExecuting(ResultExecutingContext context)
        {
            Logger.LogInformation("After Result filter");
        }
    }
}
