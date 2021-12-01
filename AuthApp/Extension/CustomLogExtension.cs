using AuthApp.Middlewares;
using Microsoft.AspNetCore.Builder;

namespace AuthApp.Extension
{
    public static class CustomLogExtension
    {
        public static IApplicationBuilder CustomLog(this IApplicationBuilder app)
            => app.UseMiddleware<CustomException>();
    }
}
