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
    //This pattern is taking into future requirement into consideration and assuming its a core project rather than POC app
    //PaymentRefundProcessor, EnrolmentProcessor, PartialRefundProcessor
    //For a POC a single AuthorisationService along with the IRepository<T> at the controller level would suffice 
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
                MerchantAccount = request.MerchantAccount,
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

            //paymentParameters.MerchantAccount, could use merchant id to isolate data
            //event sourcing to keep record of all interactions and its sequence
            await _repository.SaveAsync(payment);

            return _Mapper.Map<Payment, ProcessorResponse>(payment);
        }
    }
}