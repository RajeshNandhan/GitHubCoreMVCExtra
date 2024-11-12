using System;
using System.Threading.Tasks;
using Core.Library.ArivuTharavuThalam;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Core.MVC.Controllers
{
    public class BookController : Controller
    {
        private string accessToken;
        private string idToken;
        private string refreshToken;
        private readonly CurrentSelections currentSelections;
        private readonly BookModuleClient apiClient;

        public BookController(BookModuleClient apiClient, IOptions<CurrentSelections> currentSelections)
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
            var result = await apiClient.GetBooks(accessToken).ConfigureAwait(false);
            return View(result);
        }

        public async Task<ActionResult> Edit(int bookId)
        {
            SetToken();
            Book book = await apiClient.GetBookById(accessToken, bookId).ConfigureAwait(false);
            return View(book);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(int bookId, [FromForm] Book book)
        {
            SetToken();
            await apiClient.UpdateBookById(accessToken, bookId, book).ConfigureAwait(false);
            return RedirectToAction(nameof(Index));
        }

        private async void SetToken()
        {
            accessToken = null;
        }
    }
}