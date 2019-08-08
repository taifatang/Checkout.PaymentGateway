using Checkout.PaymentGateway.Host.Processor;

namespace Checkout.PaymentGateway.Host.Contracts
{
    public class AuthoriseRequest
    {
        public string MerchantAccount { get; set; }
        public CardDetails CardDetails { get; set; }
        public decimal Amount { get; set; }
        public string CurrencyCode { get; set; }
    }

    public class AuthoriseResponse
    {
        public string Id { get; set; }
        public PaymentStatus Status { get; set; }
        public CardDetails CardDetails { get; set; }
        public decimal Amount { get; set; }
        public string CurrencyCode { get; set; }
    }
}
