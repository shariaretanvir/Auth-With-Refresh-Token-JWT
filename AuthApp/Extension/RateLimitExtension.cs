using AspNetCoreRateLimit;
using AuthApp.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace AuthApp.Extension
{
    public static class RateLimitExtension
    {
        public static IServiceCollection ConfigureRateLimit (this IServiceCollection services)
        {
            // needed to load configuration from appsettings.json
            services.AddOptions();

            // needed to store rate limit counters and ip rules
            services.AddMemoryCache();

            //load general configuration from appsettings.json
            //services.Configure<IpRateLimitOptions>(Common.AppSettings.GetSection("IpRateLimiting"));

            //load ip rules from appsettings.json
            //services.Configure<IpRateLimitPolicies>(Common.AppSettings.GetSection("IpRateLimitPolicies"));


            //load general configuration from appsettings.json
            services.Configure<ClientRateLimitOptions>(Common.AppSettings.GetSection("ClientRateLimiting"));

            //load client rules from appsettings.json
            services.Configure<ClientRateLimitPolicies>(Common.AppSettings.GetSection("ClientRateLimitPolicies"));


            // inject counter and rules stores
            services.AddInMemoryRateLimiting();
            //services.AddDistributedRateLimiting<AsyncKeyLockProcessingStrategy>();
            //services.AddDistributedRateLimiting<RedisProcessingStrategy>();
            //services.AddRedisRateLimiting();

            // Add framework services.
            //services.AddMvc();

            // https://github.com/aspnet/Hosting/issues/793
            // the IHttpContextAccessor service is not registered by default.
            // the clientId/clientIp resolvers use it.
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            // configuration (resolvers, counter key builders)
            services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();

            

            return services;
        }
    }
}
