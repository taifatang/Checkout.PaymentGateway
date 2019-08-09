using System.Threading.Tasks;
using Checkout.PaymentGateway.Host.Contracts.Acquirers;

namespace Checkout.PaymentGateway.Host.AcquiringBank
{
    public interface IAcquirerHandler
    {
        AcquirerResponse Process(AcquirerRequest request);
    }
}