using System.Collections.Generic;
using Checkout.PaymentGateway.Host.Contracts;
using Checkout.PaymentGateway.Host.Models;

namespace Checkout.PaymentGateway.Host.Mappers
{
    public class ProcessorResponseMapper : IMap<Payment, ProcessorResponse>
    {
        public ProcessorResponse Map(Payment payment)
        {
            return new ProcessorResponse()
            {
                Payment = payment,
                Errors = payment.AcquirerStatus == "Accepted" ? null : new List<Error>()
                {
                    new Error {Code = "TakePaymentFailed", Details = "Please try again later"} //Temporary, for acquirer errors in the future
                },
            };
        }
    }
}