using Checkout.PaymentGateway.Host.Contracts;
using Checkout.PaymentGateway.Host.Models;
using Checkout.PaymentGateway.Host.Processor;

namespace Checkout.PaymentGateway.Host.AcquiringBank
{
    public interface IAcquirerHandler
    {
        AcquirerResponse Process(AcquirerRequest request);
    }
}