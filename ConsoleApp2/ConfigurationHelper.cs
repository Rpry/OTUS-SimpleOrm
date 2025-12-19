using Microsoft.Extensions.Configuration;
using System.IO;

namespace SimpleOrmApplication
{
    public static class ConfigurationHelper
    {
        private static IConfiguration _configuration;

        static ConfigurationHelper()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appSettings.json", optional: false, reloadOnChange: true);

            _configuration = builder.Build();
        }

        public static string GetConnectionString(string name)
        {
            return _configuration.GetConnectionString(name);
        }
    }
}
