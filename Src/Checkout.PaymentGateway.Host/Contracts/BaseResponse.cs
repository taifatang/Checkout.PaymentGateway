using System.Collections.Generic;

namespace Checkout.PaymentGateway.Host.Contracts
{
    public class BaseResponse
    {
        public IEnumerable<Error> Errors { get; set; }
    }
}