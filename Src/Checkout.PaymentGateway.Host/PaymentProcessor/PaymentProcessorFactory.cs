using System;
using Microsoft.Extensions.DependencyInjection;

namespace Checkout.PaymentGateway.Host.PaymentProcessor
{
    public class PaymentProcessorFactory: IPaymentProcessorFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public PaymentProcessorFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IPaymentProcessor<T> Get<T>()
        {
            return _serviceProvider.GetRequiredService<IPaymentProcessor<T>>();
        }
    }
}
