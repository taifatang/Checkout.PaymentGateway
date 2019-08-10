using Checkout.PaymentGateway.Host.Contracts;
using Checkout.PaymentGateway.Host.Contracts.Acquirers;

namespace Checkout.PaymentGateway.Host.Mappers
{
    public class AcquirerRequestMapper : IMap<AuthoriseRequest, AcquirerRequest>
    {
        public AcquirerRequest Map(AuthoriseRequest request)
        {
            return new AcquirerRequest()
            {
                PaymentOperation = PaymentOperation.Authorise,
                DesignatedAccount = request.MerchantAccount,
                CardDetails = request.CardDetails,
                Amount = request.Amount,
                CurrencyCode = request.CurrencyCode
            };
        }
    }
}