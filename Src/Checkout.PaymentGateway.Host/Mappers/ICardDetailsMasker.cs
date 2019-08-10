using Checkout.PaymentGateway.Host.Models;

namespace Checkout.PaymentGateway.Host.Mappers
{
    public interface ICardDetailsMasker
    {
        CardDetails Mask(CardDetails cardDetails);
    }
}