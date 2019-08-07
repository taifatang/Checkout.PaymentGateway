namespace Checkout.PaymentGateway.InMemoryTests.Contracts
{
    public class AuthoriseResponse: BaseResponse
    {
        public string Id { get; set; }
        public string Status { get; set; }
    }
}