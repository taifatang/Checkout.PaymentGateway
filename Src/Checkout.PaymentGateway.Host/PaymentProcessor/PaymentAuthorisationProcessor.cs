using System;
using Checkout.PaymentGateway.Host.AcquiringBank;
using Checkout.PaymentGateway.Host.Contracts;
using Checkout.PaymentGateway.Host.Mappers;
using Checkout.PaymentGateway.Host.Models;
using Checkout.PaymentGateway.Host.Processor;
using Checkout.PaymentGateway.Host.Repositories;

namespace Checkout.PaymentGateway.Host.PaymentHandler
{
    public class PaymentAuthorisationProcessor : IPaymentProcessor<AuthoriseRequest>
    {
        private readonly IRepository<Payment> _repository;
        private readonly IAcquirerHandler _acquirerHandler;
        private IMapper _Mapper;

        public ProcessorResponse ExecuteAsync(AuthoriseRequest request)
        {
            //paymentParameters.MerchantAccount
            var acquirerRequest = _Mapper.Map<AuthoriseRequest, AcquirerRequest>(request);

            var acquirerResponse = _acquirerHandler.Process(acquirerRequest);

            var payment = new Payment
            {
                Id = Guid.Empty.ToString(),
                AcquirerReference = acquirerResponse.Id,
                AcquirerStatus = acquirerResponse.Status,
                Amount = request.Amount,
                Currency = request.CurrencyCode,
                CardDetails = new CardDetails
                {
                    CardNumber = request.CardDetails.CardNumber,
                    ExpiryDate = request.CardDetails.ExpiryDate
                }
            };

            _repository.SaveAsync(payment);

            return new ProcessorResponse()
            {
                Payment = payment
            };
        }
    }

    public class ProcessorResponseMapper : IMap<Payment, ProcessorResponse>
    {
        private readonly CardNumberObscurer _cardNumberObscurer;

        public ProcessorResponseMapper(CardNumberObscurer cardNumberObscurer)
        {
            _cardNumberObscurer = cardNumberObscurer;
        }
        public ProcessorResponse Map(Payment payment)
        {
            return new ProcessorResponse()
            {
                Id = payment.Id,
                Status = payment.AcquirerStatus == "Accepted" ? PaymentStatus.Accepted : PaymentStatus.Failed,
                Payment = MaskedFields(payment)
            };
        }

        public Payment MaskedFields(Payment payment)
        {
            payment.CardDetails.CardNumber = "***";
            payment.CardDetails.SecurityCode = "***";

            return payment;
        }
    }
}