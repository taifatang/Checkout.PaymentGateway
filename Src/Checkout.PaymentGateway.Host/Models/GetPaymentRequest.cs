namespace Checkout.PaymentGateway.Host.Models
{
    public class GetPaymentRequest : IIdentifiable
    {
        public string Id { get; set; }
        public string MerchantAccount { get; set; }
    }
}