using System.Threading.Tasks;
using Checkout.PaymentGateway.Host.Contracts;
using Checkout.PaymentGateway.Host.Mappers;
using Checkout.PaymentGateway.Host.Models;
using Checkout.PaymentGateway.Host.PaymentProcessor;
using Checkout.PaymentGateway.Host.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Checkout.PaymentGateway.Host.Controllers
{
    [Route("payment/")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentProcessorFactory _paymentProcessorFactory;
        private readonly IMapper _mapper;
        private readonly IRepository<Payment> _repository;

        public PaymentController(IPaymentProcessorFactory paymentProcessorFactory, IMapper mapper, IRepository<Payment> repository)
        {
            _paymentProcessorFactory = paymentProcessorFactory;
            _mapper = mapper;
            _repository = repository;
        }

        [HttpPost, Route("authorise")]
        public async Task<IActionResult> Authorise(AuthoriseRequest request)
        {
            var processor = _paymentProcessorFactory.Get<AuthoriseRequest>();

            var result = await processor.ExecuteAsync(request);

            var response = _mapper.Map<ProcessorResponse, AuthoriseResponse>(result);

            if (response.Status == PaymentStatus.Failed)
            {
                return UnprocessableEntity(response);
            }
            return Ok(response);
        }

        [HttpGet, Route("{id}/{merchantAccount}")]
        public async Task<IActionResult> Get(string id, string merchantAccount)
        {
            //simple operation, _paymentProcessorFactory is designed for other acquirer operation refund, enrol etc
            var payment = await _repository.GetAsync(new GetPaymentRequest
            {
                Id = id,
                MerchantAccount = merchantAccount
            });

            if (payment == null)
            {
                return NotFound();
            }
            return Ok(payment);
        }
    }
}