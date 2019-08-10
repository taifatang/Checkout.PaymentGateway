using System;
using Checkout.PaymentGateway.Host.Contracts.Validations;
using FluentValidation.TestHelper;
using NUnit.Framework;

namespace Checkout.PaymentGateway.Host.UnitTests.Contracts.Validations
{
    [TestFixture]
    public class PaymentDetailsValidatorTests
    {
        private PaymentDetailsValidator _paymentDetailsValidator;

        [SetUp]
        public void SetUp()
        {
            _paymentDetailsValidator = new PaymentDetailsValidator();
        }

        [TestCase("123456789012")]
        [TestCase("1234567890123")]
        [TestCase("12345678901234")]
        [TestCase("123456789012345")]
        [TestCase("1234567890123456")]
        [TestCase("12345678901234567")]
        [TestCase("123456789012345678")]
        [TestCase("1234567890123456789")]
        public void Should_pass_when_card_number_length_is_valid(string cardNumber)
        {
            _paymentDetailsValidator.ShouldNotHaveValidationErrorFor(details => details.CardNumber, cardNumber);
        }

        [TestCase("12345678901")]
        [TestCase("12345678901234567890")]
        [TestCase("12345678901234xyz")]
        [TestCase("12345678901234%^&")]
        [TestCase("-1234567890123456")]
        [TestCase("")]
        [TestCase(null)]
        public void Should_have_error_when_card_number_is_invalid(string cardNumber)
        {
            _paymentDetailsValidator.ShouldHaveValidationErrorFor(details => details.CardNumber, cardNumber);
        }

        [TestCase("12345678901234xyz")]
        [TestCase("12345678901234%^&")]
        [TestCase("-1234567890123456")]
        public void Should_have_error_when_card_number_contains_non_integer(string cardNumber)
        {
            _paymentDetailsValidator.ShouldHaveValidationErrorFor(details => details.CardNumber, cardNumber);
        }

        [TestCase("123")]
        [TestCase("1234")]
        public void Should_pass_when_security_code_is_valid(string securityCode)
        {
            _paymentDetailsValidator.ShouldNotHaveValidationErrorFor(details => details.SecurityCode, securityCode);
        }

        [TestCase("12")]
        [TestCase("12345")]
        [TestCase("%%%")]
        [TestCase("abc")]
        [TestCase("")]
        [TestCase(null)]
        public void Should_have_error_when_security_code_length_is_invalid(string securityCode)
        {
            _paymentDetailsValidator.ShouldHaveValidationErrorFor(details => details.SecurityCode, securityCode);
        }

        [Test]
        public void Should_pass_when_expiry_date_is_valid()
        {
            var today = DateTime.Now.ToString("MMy");

            _paymentDetailsValidator.ShouldNotHaveValidationErrorFor(details => details.ExpiryDate, today);
        }

        [Test]
        public void Should_have_error_when_expiry_date_is_less_than_today( )
        {
            var expiredDate = DateTime.Now.AddMonths(-1).ToString("MMy");

            _paymentDetailsValidator.ShouldHaveValidationErrorFor(details => details.ExpiryDate, expiredDate);
        }

        [TestCase("123")]
        [TestCase("12345")]
        [TestCase("%%%%")]
        [TestCase("abcd")]
        [TestCase("")]
        [TestCase(null)]
        public void Should_have_error_when_expiry_date_is_invalid(string expiryDate)
        {
            _paymentDetailsValidator.ShouldHaveValidationErrorFor(details => details.CardNumber, expiryDate);
        }

    }
}
