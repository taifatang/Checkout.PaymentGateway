using System;
using System.Collections.Generic;
using Checkout.PaymentGateway.Host.Processor;

namespace Checkout.PaymentGateway.Host.AcquiringBank
{
    public class TestCardAcquirerHandler : IAcquirerHandler
    {
        private const string TestActivationCardNumber = "4242424242424242";
        private readonly Dictionary<string, string> _mockResponse = new Dictionary<string, string>()
        {
            { "111", "Success" },
            { "222", "Failed" }
        };
        private readonly IAcquirerHandler _innerAcquirerHandler;

        public TestCardAcquirerHandler(IAcquirerHandler innerAcquirerHandler)
        {
            _innerAcquirerHandler = innerAcquirerHandler;
        }

        public AcquirerResponse Process(PaymentParameters request)
        {
            if (request.PaymentDetails.CardNumber == TestActivationCardNumber && _mockResponse.ContainsKey(request.PaymentDetails.SecurityCode))
            {
                return new AcquirerResponse()
                {
                    Id = Guid.NewGuid().ToString(),
                    Status = _mockResponse[request.PaymentDetails.SecurityCode]
                };
            }
            return _innerAcquirerHandler.Process(request);
        }
    }
}