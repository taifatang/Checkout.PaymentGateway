namespace Checkout.PaymentGateway.Host.Models
{
    public class CardDetails
    {
        public string CardNumber { get; set; }
        public string ExpiryDate { get; set; }
        public string SecurityCode { get; set; }
    }
}