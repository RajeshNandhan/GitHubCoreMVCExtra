using System;
using System.Threading.Tasks;
using Core.Library.ArivuTharavuThalam;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Core.MVC.Controllers
{
    public class PersonController : Controller
    {
        private string accessToken;
        private string idToken;
        private string refreshToken;
        private readonly PersonModuleClient apiClient;
        private readonly CurrentSelections currentSelections;

        public PersonController(PersonModuleClient apiClient, IOptions<CurrentSelections> currentSelections)
        {
            if (currentSelections.Value != null)
                this.currentSelections = currentSelections.Value;
            else
                throw new ArgumentNullException("Current Selections to choose data context is not configured");

            this.apiClient = apiClient;
        }

        public async Task<IActionResult> Index()
        {
            SetToken(); //can be called by INTERCEPTER as code clean up
            var result = await apiClient.GetPersons(accessToken);
            return View(result);
        }

        public async Task<ActionResult> Edit(int personId)
        {
            SetToken(); //can be called by INTERCEPTER as code clean up
            Person person = await apiClient.GetPersonById(accessToken, personId).ConfigureAwait(true);
            return View(person);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(int personId, [FromForm] Person person)
        {
            SetToken();
            await apiClient.UpdatePersonById(accessToken, personId, person).ConfigureAwait(true);
            return RedirectToAction(nameof(Index));
        }

        private async void SetToken()
        {
            accessToken = null;
        }
    }
}