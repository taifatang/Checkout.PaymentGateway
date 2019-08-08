using Checkout.PaymentGateway.Host.Contracts;
using Checkout.PaymentGateway.Host.Processor;

namespace Checkout.PaymentGateway.Host.Mappers
{
    public class AuthoriseResponseMapper : IMap<ProcessorResponse, AuthoriseResponse>
    {
        private readonly CardNumberObscurer _cardNumberObscurer;

        public AuthoriseResponseMapper(CardNumberObscurer cardNumberObscurer)
        {
            _cardNumberObscurer = cardNumberObscurer;
        }

        public AuthoriseResponse Map(ProcessorResponse processorResponse)
        {
            var payment = processorResponse.Payment;

            return new AuthoriseResponse
            {
                Id = payment.Id,
                Status = payment.AcquirerStatus == "Accepted" ? PaymentStatus.Accepted : PaymentStatus.Failed,
                Amount = payment.Amount,
                CurrencyCode = payment.Currency,
                CardDetails = MaskedFields(payment.CardDetails)
            };
        }

        private CardDetails MaskedFields(CardDetails cardDetails)
        {
            cardDetails.CardNumber = _cardNumberObscurer.GetFirstSixAndLastFour(cardDetails.CardNumber);
            cardDetails.SecurityCode = "***";

            return cardDetails;
        }
    }
}