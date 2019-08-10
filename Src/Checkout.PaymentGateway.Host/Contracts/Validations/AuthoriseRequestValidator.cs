using FluentValidation;

namespace Checkout.PaymentGateway.Host.Contracts.Validations
{
    //a more complex validation mechanism may be ideal especially for card number, luhn, bin detection: card length
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
            //assuming there are restriction on the currency? 
            //or somehow business rules to make money from exchange rate margin?
            return currencyCode == "GBP";
        }
    }
}
