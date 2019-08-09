namespace Checkout.PaymentGateway.Host.Contracts.Acquirers
{
    public class AcquirerRequest
    {
        public string PaymentOperation { get; set; }
        public CardDetails CardDetails { get; set; }
        public decimal Amount { get; set; }
        public string CurrencyCode { get; set; }
    }
}