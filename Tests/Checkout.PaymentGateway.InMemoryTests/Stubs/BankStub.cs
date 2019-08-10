using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Checkout.PaymentGateway.Host.AcquiringBanks;
using Checkout.PaymentGateway.Host.Contracts.Acquirers;

namespace Checkout.PaymentGateway.InMemoryTests.Stubs
{
    public class BankStub : IAcquirerHandler
    {
        private readonly Dictionary<string, AcquirerResponse> _mockedResponse = new Dictionary<string, AcquirerResponse>()
        {
            { "111", new AcquirerResponse(){ Id = Guid.NewGuid().ToString(), Status = "Accepted"} },
            { "222", new AcquirerResponse(){ Id = Guid.NewGuid().ToString(), Status = "Failed"} }
        };

        public Task<AcquirerResponse> ProcessAsync(AcquirerRequest request)
        {
            if (_mockedResponse.TryGetValue(request.CardDetails.SecurityCode, out var response))
            {
                return Task.FromResult(response);
            }

            throw new NotImplementedException();
        }
    }
}
