namespace Checkout.PaymentGateway.InMemoryTests.Contracts
{
    public class AuthoriseResponse: BaseResponse
    {
        public string Id { get; set; }
        public string Status { get; set; }
        public CardDetails CardDetails { get; set; }
        public decimal Amount { get; set; }
        public string CurrencyCode { get; set; }
    }
}