using System.Threading.Tasks;
using Checkout.PaymentGateway.Host.Models;

namespace Checkout.PaymentGateway.Host.PaymentProcessor
{
    public interface IPaymentProcessor<T>
    {
        ProcessorResponse ExecuteAsync(T paymentRequest);
    }
}