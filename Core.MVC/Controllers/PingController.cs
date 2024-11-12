using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Core.MVC.Controllers
{
    public class PingController : Controller
    {
        private readonly AdditionalServiceSettings additionalServiceSettings;
        private ILogger<PingController> logger;
        private readonly CurrentSelections currentSelections;

        public PingController(ILogger<PingController> logger, IOptions<AdditionalServiceSettings> additionalServiceSettings, IOptions<CurrentSelections> currentSelections)
        {
            if (additionalServiceSettings.Value != null)
                this.additionalServiceSettings = additionalServiceSettings.Value;
            else
                throw new ArgumentNullException("Azure Active Directory settings not configured");

            if (currentSelections.Value != null)
                this.currentSelections = currentSelections.Value;
            else
                throw new ArgumentNullException("Current Selections to choose data context is not configured");

            this.logger = logger;
        }

        public IActionResult Index()
        {
            IDictionary<string, string> keyValuePairs = new Dictionary<string, string>
            {
                { "ProcessName", Process.GetCurrentProcess().ProcessName },
                { "HostTime", DateTime.Now.ToString() },
                { "ASPNETCORE_ENVIRONMENT", Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") },
                { "Additional Service - BaseUrl", additionalServiceSettings.BaseUrl },
                { "Additional Service - BooksEndPoint", additionalServiceSettings.BooksEndPoint },
                { "Additional Service - PersonEndpoint", additionalServiceSettings.PersonEndpoint }
            };

            string value = JsonConvert.SerializeObject(keyValuePairs);

            ViewBag.PrintMessage = value;

            Log.Information(value);

            return View();
        }
    }
}