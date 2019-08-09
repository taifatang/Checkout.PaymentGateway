namespace Checkout.PaymentGateway.Host.Models
{
    public class Payment
    {
        public string Id { get; set; }
        public string AcquirerReference { get; set; }
        public string AcquirerStatus { get; set; }
        public CardDetails CardDetails { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
    }
}