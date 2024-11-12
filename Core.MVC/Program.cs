using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.Text;

namespace Core.MVC
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //var builder = CreateHostBuilderDev(args);
            var builder = CreateHostBuilder(args);

            try
            {
                var host = builder.Build();
                host.Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Program Startup");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilderDev(string[] args) =>
           Host.CreateDefaultBuilder(args)
               .ConfigureWebHostDefaults(webBuilder =>
               {
                   webBuilder.UseStartup<Startup>();
               });

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            if (string.IsNullOrEmpty(environment))
                environment = "Development";

            IConfiguration config = new ConfigurationBuilder()
            .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true)
            .Build();

            string kestralEndpointUrl = config.GetSection("Kestrel:Endpoints:Http:Url").Value;

            return Host.CreateDefaultBuilder(args)
                    .ConfigureWebHostDefaults(webBuilder =>
                    {
                        webBuilder
                        //.ConfigureLogging((hostingContext, logging) =>
                        //{
                        //    //logging.AddEventLog();
                        //})
                        //.ConfigureAppConfiguration((context, config) =>
                        //{
                        //    Log.Logger = new LoggerConfiguration()
                        //    .ReadFrom.Configuration(config.Build())
                        //    .CreateLogger();
                        //})
                        //.UseSerilog()
                        .UseKestrel(option =>
                        {
                            StringBuilder sb = new StringBuilder();
                            foreach (var arg in args)
                            {
                                sb.Append(arg);
                                sb.Append(Environment.NewLine);
                            }
                            Log.Information(sb.ToString());

                            Log.Information($"ASPNETCORE_ENVIRONMENT - {environment}");
                            Log.Information($"[USE PING]- Kestrel:Endpoints:Http:Url - {config.GetSection("Kestrel:Endpoints:Http:Url").Value}");
                        })
                        .UseUrls(kestralEndpointUrl)
                        .UseStartup<Startup>();
                    });//.UseWindowsService();
        }
    }
}
