namespace Checkout.PaymentGateway.Host.Models
{
    public interface IIdentifiable
    {
        string Id { get; set; }
        string MerchantAccount { get; set; }
    }
}