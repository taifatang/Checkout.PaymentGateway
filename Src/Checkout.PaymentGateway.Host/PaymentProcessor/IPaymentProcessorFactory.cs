namespace Checkout.PaymentGateway.Host.PaymentProcessor
{
    public interface IPaymentProcessorFactory
    {
        IPaymentProcessor<T> Get<T>();
    }
}