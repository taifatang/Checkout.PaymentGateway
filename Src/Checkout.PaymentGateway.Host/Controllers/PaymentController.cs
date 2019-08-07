using System.Linq;
using Checkout.PaymentGateway.Host.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Checkout.PaymentGateway.Host.Controllers
{
    [Route("payment/")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        [HttpPost, Route("authorise")]
        public IActionResult Authorise(AuthoriseRequest request)
        {
            if (!ModelState.IsValid)
            {
                var response = new BaseResponse()
                {
                    Errors = ModelState.Select(x => new Error()
                    {
                        Code = x.Key,
                        Details = x.Value.AttemptedValue
                    })
                };
                return BadRequest(response);
            }
            return null;
        }
    }
}