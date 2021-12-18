using AuthApp.Filters;
using AuthApp.Utilities;
using Microsoft.Extensions.DependencyInjection;

namespace AuthApp.Extension
{
    public static class CustomServiceExtensions
    {
        public static IServiceCollection CustomServices(this IServiceCollection services)
        {
            services.AddScoped<IToken, Token>();
            //services.AddScoped<CommonRequestFilter>();
            //services.AddScoped<CustomResultFilterAttribute>();
            services.AddScoped<LoggingFilterAttribute>();
            services.AddScoped<ModelValidationFilterAttribute>();
            return services;
        }
    }
}
