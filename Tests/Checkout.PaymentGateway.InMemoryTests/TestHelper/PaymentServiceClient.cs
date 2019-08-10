using System.Net.Http;
using System.Threading.Tasks;
using Checkout.PaymentGateway.InMemoryTests.Contracts;
using Newtonsoft.Json;

namespace Checkout.PaymentGateway.InMemoryTests.TestHelper
{
    public class PaymentServiceClient
    {
        private readonly HttpClient _httpClient;

        public PaymentServiceClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ApiResponse<AuthoriseResponse>> Authorise(AuthoriseRequest request)
        {
            var response = await _httpClient.PostAsJsonAsync("payment/authorise", request);

            var content = await response.Content.ReadAsStringAsync();

            return new ApiResponse<AuthoriseResponse>
            {
                Content = JsonConvert.DeserializeObject<AuthoriseResponse>(content),
                HttpStatusCode = response.StatusCode,
                Header = response.Headers
            };
        }
    }
}