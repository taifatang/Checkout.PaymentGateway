using Checkout.PaymentGateway.Host.Contracts;
using Checkout.PaymentGateway.Host.Models;
using CardDetails = Checkout.PaymentGateway.Host.Contracts.CardDetails;

namespace Checkout.PaymentGateway.Host.Mappers
{
    public class AuthoriseResponseMapper : IMap<ProcessorResponse, AuthoriseResponse>
    {
        public AuthoriseResponse Map(ProcessorResponse processorResponse)
        {
            var payment = processorResponse.Payment;

            return new AuthoriseResponse
            {
                Id = payment.Id,
                Status = payment.AcquirerStatus == "Accepted" ? PaymentStatus.Accepted : PaymentStatus.Failed,
                Amount = payment.Amount,
                CurrencyCode = payment.Currency,
                CardDetails = new CardDetails
                {
                    CardNumber = processorResponse.Payment.CardDetails.CardNumber,
                    ExpiryDate = processorResponse.Payment.CardDetails.ExpiryDate
                },
                Errors = payment.AcquirerStatus == "Failed" ? processorResponse.Errors : null
            };
        }
    }
}