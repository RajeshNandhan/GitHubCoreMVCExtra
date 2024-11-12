using Core.Library.ArivuTharavuThalam;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Core.MVC
{
    public class BookModuleClient
    {
        private readonly IHttpClientFactory httpClientFactory;
        private readonly AdditionalServiceSettings additionalServiceSettings;
        private readonly HttpClient httpClient;

        public BookModuleClient(IHttpClientFactory httpClientFactory, IOptions<AdditionalServiceSettings> additionalServiceSettings)
        {
            this.httpClientFactory = httpClientFactory;

            if (this.httpClientFactory != null)
                httpClient = this.httpClientFactory.CreateClient();
            else
                throw new ArgumentException("HTTP client");

            if (additionalServiceSettings != null)
                this.additionalServiceSettings = additionalServiceSettings.Value;
            else
                throw new ArgumentException("Principle Service Endpoint Value configuration");
        }

        public async Task<IEnumerable<Book>> GetBooks(string accessToken)
        {
            string url = additionalServiceSettings.BaseUrl + additionalServiceSettings.BooksEndPoint;

            using (HttpResponseMessage response = await httpClient.GetAsync(url))
            {
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                    var resultDeSerialized = JsonConvert.DeserializeObject<List<Book>>(result);
                    return resultDeSerialized;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

        public async Task<Book> GetBookById(string accessToken, int bookId)
        {
            string url = additionalServiceSettings.BaseUrl + additionalServiceSettings.BooksEndPoint + "/" + bookId;

            using (HttpResponseMessage response = await httpClient.GetAsync(url))
            {
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                    var resultDeSerialized = JsonConvert.DeserializeObject<Book>(result);
                    return resultDeSerialized;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

        public async Task<Book> UpdateBookById(string accessToken, int bookId, Book book)
        {
            string url = additionalServiceSettings.BaseUrl + additionalServiceSettings.BooksEndPoint + "/" + bookId;

            var bookContent = new StringContent(JsonConvert.SerializeObject(book), Encoding.UTF8, "application/json");

            using (HttpResponseMessage response = await httpClient.PutAsync(url, bookContent))
            {
                if (response.IsSuccessStatusCode)
                {
                    return book;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }
    }
}
