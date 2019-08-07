using System;
using System.Globalization;
using FluentValidation;

namespace Checkout.PaymentGateway.Host.Contracts.Validations
{
    public class PaymentDetailsValidator : AbstractValidator<PaymentDetails>
    {
        public PaymentDetailsValidator()
        {
            RuleFor(p => p.CardNumber).NotEmpty().Length(12, 19).WithMessage("Card number must be 12 to 19 digit in length");
            RuleFor(p => p.CardNumber).Matches(@"^\d+$").WithMessage("Card number must contain digit only");
            RuleFor(p => p.SecurityCode).NotEmpty().Length(3, 4).WithMessage("Security code must be 3 or 4 digits in length");
            RuleFor(p => p.SecurityCode).Matches(@"^\d+$").WithMessage("Security code must contain digit only");
            RuleFor(p => p.ExpiryDate).Must(GreaterThanToday).WithMessage("Expiry date must be greater than today");
            RuleFor(p => p.ExpiryDate).NotEmpty().Length(4).Matches(@"^\d+$").WithMessage("Expiry date is invalid");

        }
        public bool GreaterThanToday(string expiryDate)
        {
            if (DateTime.TryParseExact(
                expiryDate, "MMyy",
                CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal,
                out var expiration))
            {
                var today = DateTime.Now;

                return expiration.Month >= today.Month
                       && expiration.Year >= today.Year;
            }
            return false;
        }
    }
}