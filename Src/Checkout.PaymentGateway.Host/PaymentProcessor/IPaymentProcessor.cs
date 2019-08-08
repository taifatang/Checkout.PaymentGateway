using Checkout.PaymentGateway.Host.Processor;

namespace Checkout.PaymentGateway.Host.PaymentHandler
{
    public interface IPaymentProcessor<T>
    {
        ProcessorResponse ExecuteAsync(T paymentRequest);
    }
}