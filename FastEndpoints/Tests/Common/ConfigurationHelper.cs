using Microsoft.Extensions.Configuration;

namespace Tests.Common
{
    public static class ConfigurationHelper
    {
        public static IConfiguration GetConfiguration() => 
            new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();
    }
}
