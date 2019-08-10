using FluentValidation;

namespace Checkout.PaymentGateway.Host.Contracts.Validations
{
    public class AuthoriseRequestValidator: AbstractValidator<AuthoriseRequest>
    {
        public AuthoriseRequestValidator()
        {
            RuleFor(request => request.MerchantAccount).NotEmpty().WithMessage("Merchant Account must not be empty");
            RuleFor(request => request.Amount).GreaterThan(0).WithMessage("Amount must be greater than 0");
            RuleFor(request => request.CurrencyCode).Must(SupportedCurrencyCode).WithMessage("Currency is not supported");

            RuleFor(request => request.CardDetails).NotNull().WithMessage("Please provide payment details");
            RuleFor(request => request.CardDetails).InjectValidator();
        }

        public bool SupportedCurrencyCode(string currencyCode)
        {
            return currencyCode == "GBP";
        }
    }
}
