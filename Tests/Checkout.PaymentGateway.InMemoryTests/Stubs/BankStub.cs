using System;
using System.Collections.Generic;
using Checkout.PaymentGateway.Host.AcquiringBank;
using Checkout.PaymentGateway.Host.Processor;

namespace Checkout.PaymentGateway.InMemoryTests.Stubs
{
    public class BankStub : IAcquirerHandler
    {
        private readonly Dictionary<string, AcquirerResponse> _mockedResponse = new Dictionary<string, AcquirerResponse>()
        {
            { "111", new AcquirerResponse(){ Id = Guid.NewGuid().ToString(), Status = "Success"} },
            { "222", new AcquirerResponse(){ Id = Guid.NewGuid().ToString(), Status = "Failed"} }
        };

        public AcquirerResponse Process(PaymentParameters request)
        {
            if (_mockedResponse.TryGetValue(request.PaymentDetails.SecurityCode, out var response))
            {
                return response;
            }

            throw new NotImplementedException();
        }
    }
}
