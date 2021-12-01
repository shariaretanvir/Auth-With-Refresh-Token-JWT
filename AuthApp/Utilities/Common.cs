using Microsoft.Extensions.Configuration;
using System.IO;

namespace AuthApp.Utilities
{
    public class Common
    {
        private static IConfiguration config;
        public static IConfiguration AppSettings {
            get
            {
                var builder = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json");
                config = builder.Build();
                return config;
            }
        }
    }
}
