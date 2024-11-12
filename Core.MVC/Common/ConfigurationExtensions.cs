using Microsoft.Extensions.Configuration;
using System;

namespace Core.MVC
{
    public static class ConfigurationExtensions
    {
        /// <summary>
        /// NOT USED
        /// </summary>
        /// <param name="Configuration"></param>
        /// <returns></returns>
        public static IConfiguration GetConfigurationBasedOnAppsettingFile(this ConfigurationBuilder Configuration)
        {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            if (string.IsNullOrEmpty(environment))
                environment = "Development";

            IConfiguration config = new ConfigurationBuilder()
            .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true)
            .Build();

            return config;
        }
    }
}