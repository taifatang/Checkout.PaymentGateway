namespace Checkout.PaymentGateway.Host.Contracts
{
    public class AuthoriseBaseResponse : BaseResponse
    {
        public string Id { get; set; }
        public string Status { get; set; }
    }
}