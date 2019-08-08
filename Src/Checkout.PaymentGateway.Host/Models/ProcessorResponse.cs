using Checkout.PaymentGateway.Host.Models;

namespace Checkout.PaymentGateway.Host.Processor
{
    //Depending on the request processed, in the future this may contain more info
    public class ProcessorResponse
    {
        public Payment Payment { get; set; }
    }
}