using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Checkout.PaymentGateway.Host.Contracts.Acquirers;

namespace Checkout.PaymentGateway.Host.AcquiringBanks
{
    public class FakeBarclaysBankAcquirerHandler : IAcquirerHandler
    {
        private const string TestActivationCardNumber = "4242424242424242";
        private readonly Dictionary<string, string> _mockResponses = new Dictionary<string, string>()
        {
            { "111", "Accepted" },
            { "222", "Failed" }
        };

        public Task<AcquirerResponse> ProcessAsync(AcquirerRequest request)
        {
            if (request.CardDetails.CardNumber == TestActivationCardNumber && _mockResponses.ContainsKey(request.CardDetails.SecurityCode))
            {
                return Task.FromResult(new AcquirerResponse
                {
                    Id = Guid.NewGuid().ToString(),
                    Status = _mockResponses[request.CardDetails.SecurityCode]
                });
            }

            //fail anything not expected for now
            return Task.FromResult(new AcquirerResponse
            {
                Id = Guid.NewGuid().ToString(),
                Status = "Failed"
            });
        }
    }
}