using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.IO;
using System;
using System.IO;
using System.Net;
using System.Text;
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

                //note that we can not modify the response if there is already data populated. if response data is not loaded then we
                //can create response like that.
                if (context.Response.StatusCode == (int)HttpStatusCode.Unauthorized)
                {
                    await PrepareResponse(context);
                }
            }
            catch (System.Exception e)
            {
                logger.LogError(e.Message);
                await PrepareErrorData(context, e);
            }
        }


        private async Task PrepareResponse(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            var response = JsonSerializer.Serialize(new
            {
                StatusCode = context.Response.StatusCode,
                Message = "Provide token please"
            });
            //context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(response);
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
