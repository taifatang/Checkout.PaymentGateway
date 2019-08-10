using System;
using System.Threading.Tasks;
using Checkout.PaymentGateway.Host.AcquiringBanks;
using Checkout.PaymentGateway.Host.Contracts;
using Checkout.PaymentGateway.Host.Contracts.Acquirers;
using Checkout.PaymentGateway.Host.Mappers;
using Checkout.PaymentGateway.Host.Models;
using Checkout.PaymentGateway.Host.Repositories;
using CardDetails = Checkout.PaymentGateway.Host.Models.CardDetails;

namespace Checkout.PaymentGateway.Host.PaymentProcessor
{
    public class PaymentAuthorisationProcessor : IPaymentProcessor<AuthoriseRequest>
    {
        private readonly IRepository<Payment> _repository;
        private readonly IAcquirerHandler _acquirerHandler;
        private readonly ICardDetailsMasker _cardDetailsMasker;
        private IMapper _Mapper;

        public PaymentAuthorisationProcessor(IRepository<Payment> repository, IAcquirerHandler acquirerHandler, ICardDetailsMasker cardDetailsMasker, IMapper mapper)
        {
            _repository = repository;
            _acquirerHandler = acquirerHandler;
            _cardDetailsMasker = cardDetailsMasker;
            _Mapper = mapper;
        }

        public async Task<ProcessorResponse> ExecuteAsync(AuthoriseRequest request)
        {

            var acquirerRequest = _Mapper.Map<AuthoriseRequest, AcquirerRequest>(request);

            var acquirerResponse = await _acquirerHandler.ProcessAsync(acquirerRequest);

            var payment = new Payment
            {
                Id = Guid.NewGuid().ToString(),
                AcquirerReference = acquirerResponse.Id,
                AcquirerStatus = acquirerResponse.Status,
                Amount = request.Amount,
                Currency = request.CurrencyCode,
                CardDetails = _cardDetailsMasker.Mask(new CardDetails
                {
                    CardNumber = request.CardDetails.CardNumber,
                    ExpiryDate = request.CardDetails.ExpiryDate
                })
            };

            //paymentParameters.MerchantAccount, maybe used to isolate other merchant especially database table?
            await _repository.SaveAsync(payment);

            return _Mapper.Map<Payment, ProcessorResponse>(payment);
        }
    }
}