using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PDF.Demo.Api.Business.Abstractions;
using PDF.Demo.Api.Configuration;
using PDF.Demo.Api.Models;

namespace PDF.Demo.Api.Business.DataGateway
{
    public class FormsApiClient : IFormsApiClient
    {
        private readonly HttpClient httpClient;
        private readonly AppSettings appSettings;

        public FormsApiClient(
            HttpClient httpClient,
            AppSettings appSettings)
        {
            this.httpClient = httpClient;
            this.appSettings = appSettings;
        }

        public async Task<FormResult> GetById(Guid id)
        {
            var uri = $"{appSettings.FormsApiUrl}/v1/forms";

            var responseString = await httpClient.GetStringAsync(uri);

            var formResult = JsonConvert.DeserializeObject<FormResult>(responseString);

            return formResult;
        }
    }
}
