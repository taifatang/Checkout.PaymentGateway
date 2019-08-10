using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Checkout.PaymentGateway.Host.Contracts.Acquirers;
using Microsoft.Extensions.Logging;

namespace Checkout.PaymentGateway.Host.AcquiringBanks
{
    public class FakeBarclaysBankAcquirerHandler : IAcquirerHandler
    {
        //majority of the logs are captured by the request and response middle including exceptions
        //it would have more logs when there are more moving parts
        private ILogger<FakeBarclaysBankAcquirerHandler> _logger;
        private const string TestActivationCardNumber = "4242424242424242";
        private readonly Dictionary<string, string> _mockResponses = new Dictionary<string, string>()
        {
            { "111", "Accepted" },
            { "222", "Failed" }
        };

        public FakeBarclaysBankAcquirerHandler(ILogger<FakeBarclaysBankAcquirerHandler> logger)
        {
            _logger = logger;
        }

        public Task<AcquirerResponse> ProcessAsync(AcquirerRequest request)
        {
            //have a dedicated routing class in the future for business rules.
            _logger.LogInformation($"Routing {request.PaymentOperation} for {request.DesignatedAccount} to Barclays as is trigger business rule: cheaper processing fees");

            //fail anything not expected for now
            var response = new AcquirerResponse()
            {
                Id = Guid.NewGuid().ToString(),
                Status = "Failed"
            };

            if (request.CardDetails.CardNumber == TestActivationCardNumber && _mockResponses.ContainsKey(request.CardDetails.SecurityCode))
            {
                _logger.LogInformation($"{request.PaymentOperation} for {request.DesignatedAccount} is activating test scenario");

                response.Status = _mockResponses[request.CardDetails.SecurityCode];
            }

            _logger.LogInformation($"{nameof(FakeBarclaysBankAcquirerHandler)} is return {response.Status} with reference id {response.Id}");

            return Task.FromResult(response);
        }
    }
}