using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Checkout.PaymentGateway.Host.Contracts.Acquirers;

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

        public Task<AcquirerResponse> ProcessAsync(AcquirerRequest request)
        {
            if (request.CardDetails.CardNumber == TestActivationCardNumber && _mockResponse.ContainsKey(request.CardDetails.SecurityCode))
            {
                return Task.FromResult(new AcquirerResponse
                {
                    Id = Guid.NewGuid().ToString(),
                    Status = _mockResponse[request.CardDetails.SecurityCode]
                });
            }
            return _innerAcquirerHandler.ProcessAsync(request);
        }
    }
}