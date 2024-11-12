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
    public class PersonModuleClient
    {
        private readonly IHttpClientFactory httpClientFactory;
        private readonly AdditionalServiceSettings additionalServiceSettings;
        private readonly HttpClient httpClient;

        public PersonModuleClient(IHttpClientFactory httpClientFactory, IOptions<AdditionalServiceSettings> additionalServiceSettings)
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

        public async Task<IEnumerable<Person>> GetPersons(string accessToken)
        {
            string url = additionalServiceSettings.BaseUrl + additionalServiceSettings.PersonEndpoint;

            using (HttpResponseMessage response = await httpClient.GetAsync(url))
            {
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                    var resultDeSerialized = JsonConvert.DeserializeObject<List<Person>>(result);
                    return resultDeSerialized;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

        public async Task<Person> GetPersonById(string accessToken, int personId)
        {
            string url = additionalServiceSettings.BaseUrl + additionalServiceSettings.PersonEndpoint + "/" + personId;

            using (HttpResponseMessage response = await httpClient.GetAsync(url))
            {
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                    var resultDeSerialized = JsonConvert.DeserializeObject<Person>(result);
                    return resultDeSerialized;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

        public async Task<Person> UpdatePersonById(string accessToken, int personId, Person person)
        {
            string url = additionalServiceSettings.BaseUrl + additionalServiceSettings.PersonEndpoint + "/" + personId;

            var personContent = new StringContent(JsonConvert.SerializeObject(person), Encoding.UTF8, "application/json");

            using (HttpResponseMessage response = await httpClient.PutAsync(url, personContent))
            {
                if (response.IsSuccessStatusCode)
                {
                    return person;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }
    }
}
