using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System;

namespace Core.MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHttpClientFactory httpClientFactory;
        private readonly HttpClient httpClient;

        public HomeController(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;

            if (this.httpClientFactory != null)
                httpClient = this.httpClientFactory.CreateClient();
            else
                throw new ArgumentException("HTTP client");
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
