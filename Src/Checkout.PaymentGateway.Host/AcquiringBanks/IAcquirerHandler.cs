using System.Threading.Tasks;
using Checkout.PaymentGateway.Host.Contracts.Acquirers;

namespace Checkout.PaymentGateway.Host.AcquiringBanks
{
    public interface IAcquirerHandler
    {
        Task<AcquirerResponse> ProcessAsync(AcquirerRequest request);
    }
}