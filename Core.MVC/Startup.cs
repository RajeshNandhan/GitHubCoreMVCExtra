using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Core.MVC
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        /// <summary>
        /// ConfigureServices
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            ConfigureServices_Client(services);
            ConfigureServices_ConfigurationSettings(services);

            services.AddHttpClient();

            var telemetryInstrumentationKey = Configuration["CurrentSelections:TelemetryInstrumentationKey"];

            // The following line enables Application Insights telemetry collection.
            services.AddApplicationInsightsTelemetry(telemetryInstrumentationKey);

            services.AddControllersWithViews();
        }

        public void ConfigureServices_Client(IServiceCollection services)
        {
            services.AddTransient<BookModuleClient, BookModuleClient>();
            services.AddTransient<PersonModuleClient, PersonModuleClient>();
        }

        public void ConfigureServices_ConfigurationSettings(IServiceCollection services)
        {
            services.Configure<AdditionalServiceSettings>(options => Configuration.GetSection("AdditionalServiceSettings").Bind(options));
            var CurrentSelections = Configuration.GetSection("CurrentSelections");
            services.Configure<CurrentSelections>(CurrentSelections);
        }

        /// <summary>
        /// COnfigure
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment() || env.EnvironmentName == "Local" || env.EnvironmentName == "IISExpress" || env.EnvironmentName == "Production")
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            // Enable the cookie policy middleware before authentication
            app.UseCookiePolicy();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

                endpoints.MapDefaultControllerRoute().RequireAuthorization();
            });
        }
    }
}