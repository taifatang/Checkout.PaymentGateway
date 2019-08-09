using System.Collections.Generic;
using Checkout.PaymentGateway.Host.Contracts;

namespace Checkout.PaymentGateway.Host.Models
{
    //Depending on the request processed, in the future this may contain more info
    public class ProcessorResponse
    {
        public Payment Payment { get; set; }
    }
}