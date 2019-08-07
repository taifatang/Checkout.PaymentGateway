using Checkout.PaymentGateway.Host.Contracts.Validations;
using FluentValidation.TestHelper;
using NUnit.Framework;

namespace Checkout.PaymentGateway.Host.UnitTests.Contracts.Validations
{
    [TestFixture]
    public class AuthoriseRequestValidatorTests
    {
        private AuthoriseRequestValidator _validator;

        [SetUp]
        public void SetUp()
        {
            _validator = new AuthoriseRequestValidator();
        }

        [TestCase(null)]
        [TestCase("")]
        public void Should_have_error_when_merchant_account_is_empty(string merchantAccount)
        {
            _validator.ShouldHaveValidationErrorFor(a => a.MerchantAccount, merchantAccount);
        }

        [Test]
        public void Should_not_have_error_when_merchant_account_is_supplied()
        {
            _validator.ShouldNotHaveValidationErrorFor(a => a.MerchantAccount, "CheckoutCOM");
        }

        [Test]
        public void Should_have_error_for_unsupported_currency()
        {
            _validator.ShouldHaveValidationErrorFor(a => a.CurrencyCode, "USD");
        }

        [Test]
        public void Should_not_have_error_for_supported_currency()
        {
            _validator.ShouldNotHaveValidationErrorFor(a => a.CurrencyCode, "GBP");
        }

        [TestCase(0)]
        [TestCase(-1)]
        public void Should_have_error_when_amount_is_invalid(decimal amount)
        {
            _validator.ShouldHaveValidationErrorFor(a => a.Amount, amount);
        }

        [Test]
        public void Should_not_have_error_when_amount_is_valid()
        {
            _validator.ShouldNotHaveValidationErrorFor(a => a.Amount, decimal.MaxValue);
        }
    }
}