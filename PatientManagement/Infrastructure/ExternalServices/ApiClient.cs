using System.Net.Http.Headers;

namespace PatientManagement.Infrastructure.ExternalServices
{
    public sealed class ApiClient : IApiClient
    {
        private readonly HttpClient _httpClient;

        public ApiClient()
        {
            var handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
            };

            _httpClient = new HttpClient(handler);
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<string> GetAsync(string uri, CancellationToken cancellationToken)
        {
            using var response = await _httpClient.GetAsync(uri, cancellationToken);

            if (response?.StatusCode != System.Net.HttpStatusCode.OK)
                return string.Empty;

            return await response.Content.ReadAsStringAsync(cancellationToken);
        }
    }
}
