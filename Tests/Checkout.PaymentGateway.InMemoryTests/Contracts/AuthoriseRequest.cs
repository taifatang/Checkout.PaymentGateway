namespace Checkout.PaymentGateway.InMemoryTests
{
    public class AuthoriseRequest
    {
        public string MerchantAccount { get; set; }
        public PaymentDetails PaymentDetails { get; set; }
        public decimal Amount { get; set; }
        public string CurrencyCode { get; set; }
    }
}