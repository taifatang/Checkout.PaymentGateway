using System.Linq;
using Checkout.PaymentGateway.Host.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Checkout.PaymentGateway.Host.Attirubtes
{
    public class ContractValidationAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {

                var response = new BaseResponse()
                {
                    Errors = context.ModelState.Select(x => new Error()
                    {
                        Code = x.Key,
                        Details = string.Join(", ", x.Value.Errors.Select(y => y.ErrorMessage))
                    })
                };
                context.Result = new BadRequestObjectResult(response);
            }
        }
    }
}
