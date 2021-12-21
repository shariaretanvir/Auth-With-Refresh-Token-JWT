using AspNetCoreRateLimit;
using AuthApp.Utilities;
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
            services.Configure<IpRateLimitOptions>(Common.AppSettings.GetSection("IpRateLimiting"));

            //load ip rules from appsettings.json
            services.Configure<IpRateLimitPolicies>(Common.AppSettings.GetSection("IpRateLimitPolicies"));

            // inject counter and rules stores
            services.AddInMemoryRateLimiting();
            //services.AddDistributedRateLimiting<AsyncKeyLockProcessingStrategy>();
            //services.AddDistributedRateLimiting<RedisProcessingStrategy>();
            //services.AddRedisRateLimiting();

            // Add framework services.
            //services.AddMvc();

            // configuration (resolvers, counter key builders)
            services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();


            return services;
        }
    }
}
