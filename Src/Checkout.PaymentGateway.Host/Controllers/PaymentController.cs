﻿using System.Threading.Tasks;
using Checkout.PaymentGateway.Host.Contracts;
using Checkout.PaymentGateway.Host.Mappers;
using Checkout.PaymentGateway.Host.Models;
using Checkout.PaymentGateway.Host.PaymentProcessor;
using Microsoft.AspNetCore.Mvc;

namespace Checkout.PaymentGateway.Host.Controllers
{
    [Route("payment/")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentProcessorFactory _paymentProcessorFactory;
        private readonly IMapper _mapper;

        public PaymentController(IPaymentProcessorFactory paymentProcessorFactory, IMapper mapper)
        {
            _paymentProcessorFactory = paymentProcessorFactory;
            _mapper = mapper;
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

        //[HttpGet, Route("{id}")]
        //public IActionResult Get(string id, GetPaymentRequest request)
        //{
        //    var x = _paymentProcessorFactory.Get<GetPaymentRequest>();

        //    return Ok();
        //}
    }
}