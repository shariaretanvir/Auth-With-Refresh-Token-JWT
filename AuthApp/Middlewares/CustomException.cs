using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace AuthApp.Middlewares
{
    public class CustomException
    {
        private readonly ILogger<CustomException> logger;
        private readonly RequestDelegate next;
        public CustomException(RequestDelegate next,ILogger<CustomException> logger)
        {
            this.logger = logger;
            this.next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await next.Invoke(context);
            }
            catch (System.Exception e)
            {
                logger.LogError(e.Message);
                await PrepareErrorData(context, e);
            }
        }

        private async Task PrepareErrorData(HttpContext context, Exception e)
        {
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.Response.ContentType = "application/json";
            var response = JsonSerializer.Serialize(new
            {
                StatusCode = context.Response.StatusCode,
                ErrorMessage = e.Message
            });
            await context.Response.WriteAsync(response);
        }
    }
}
