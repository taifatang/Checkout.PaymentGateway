using System.Net;
using System.Net.Http.Headers;

namespace Checkout.PaymentGateway.InMemoryTests.TestHelper
{
    public class ApiResponse<T>
    {
        public T Content { get; set; }
        public HttpStatusCode HttpStatusCode { get; set; }
        public HttpResponseHeaders Header { get; set; }
    }
}