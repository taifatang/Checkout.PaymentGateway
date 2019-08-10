using Checkout.PaymentGateway.Host.Models;

namespace Checkout.PaymentGateway.Host.Contracts
{
    public class AuthoriseResponse : BaseResponse
    {
        public string Id { get; set; }
        public PaymentStatus Status { get; set; }
        public CardDetails CardDetails { get; set; }
        public decimal Amount { get; set; }
        public string CurrencyCode { get; set; }
    }
}