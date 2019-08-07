using System.Collections.Generic;

namespace Checkout.PaymentGateway.InMemoryTests.Contracts
{
    public class BaseResponse
    {
        public IEnumerable<Error> Errors { get; set; }
    }
}