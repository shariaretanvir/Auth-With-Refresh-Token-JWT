using Microsoft.Extensions.DependencyInjection;

namespace AuthApp.Extension
{
    public static class CORSExtensions
    {
        public static void CORSExtension(this IServiceCollection service)
        {
            service.AddCors(options =>
            {
                options.AddDefaultPolicy(c =>
                {
                    c.WithOrigins("https://localhost:5001", "https://localhost:5002")
                    .AllowAnyHeader()
                    .AllowAnyMethod();
                });
            });
        }

    }
}
