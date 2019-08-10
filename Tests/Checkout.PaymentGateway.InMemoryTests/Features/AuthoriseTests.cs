using System;
using System.Net;
using System.Threading.Tasks;
using Checkout.PaymentGateway.InMemoryTests.Contracts;
using Checkout.PaymentGateway.InMemoryTests.TestHelper;
using NUnit.Framework;

namespace Checkout.PaymentGateway.InMemoryTests.Features
{
    [TestFixture]
    public class AuthoriseTests : TestBase
    {
        [Test]
        public async Task Authorise_a_payment_successfully()
        {
            var expectedSuccessRequest = new AuthoriseRequest()
            {
                CardDetails = new CardDetails()
                {
                    CardNumber = "424242424242",
                    ExpiryDate = "0129",
                    SecurityCode = "111"
                },
                MerchantAccount = "merchant_account",
                Amount = 25.00m,
                CurrencyCode = "GBP"
            };

            var response = await PaymentServiceClient.Authorise(expectedSuccessRequest);

            Assert.That(response.HttpStatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.NotNull(response.Content.Id);
            Assert.That(response.Content.Id, Is.Not.EqualTo("psp_reference_id"));
            Assert.That(response.Content.Status, Is.EqualTo("Accepted"));

            ThenValidPaymentAndOrderBody(expectedSuccessRequest, response.Content);
        }

        [Test]
        public async Task Payment_authorisation_failed()
        {
            var expectedRejectionRequest = new AuthoriseRequest()
            {
                CardDetails = new CardDetails()
                {
                    CardNumber = "424242424242",
                    ExpiryDate = "0129",
                    SecurityCode = "222"
                },
                MerchantAccount = "merchant_account",
                Amount = 25.00m,
                CurrencyCode = "GBP"
            };

            var response = await PaymentServiceClient.Authorise(expectedRejectionRequest);

            Assert.That(response.HttpStatusCode, Is.EqualTo(HttpStatusCode.UnprocessableEntity));
            Assert.NotNull(response.Content.Id);
            Assert.That(response.Content.Id, Is.Not.EqualTo("psp_reference_id"));
            Assert.That(response.Content.Status, Is.EqualTo("Failed"));
            Assert.IsNotEmpty(response.Content.Errors);

            ThenValidPaymentAndOrderBody(expectedRejectionRequest, response.Content);
        }

        [Test]
        public async Task Payment_authorisation_with_invalid_payment()
        {
            var invalidRequest = new AuthoriseRequest();

            var response = await PaymentServiceClient.Authorise(invalidRequest);

            Assert.That(response.HttpStatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            Assert.IsNotEmpty(response.Content.Errors);
        }

        public void ThenValidPaymentAndOrderBody(AuthoriseRequest request, AuthoriseResponse response)
        {
            Assert.That(response.Amount, Is.EqualTo(request.Amount));
            Assert.That(response.CurrencyCode, Is.EqualTo(request.CurrencyCode));
            Assert.That(response.CardDetails.CardNumber, Is.EqualTo(GetFirstSixAndLastFour(response.CardDetails.CardNumber)));
            Assert.IsNull(response.CardDetails.SecurityCode);
            Assert.That(response.CardDetails.ExpiryDate, Is.EqualTo(request.CardDetails.ExpiryDate));
        }

        private string GetFirstSixAndLastFour(string cardNumber)
        {
            if (string.IsNullOrEmpty(cardNumber))
            {
                return cardNumber;
            }

            var firstDigits = cardNumber.Substring(0, 6);
            var lastDigits = cardNumber.Substring(cardNumber.Length - 4, 4);

            var requiredMask = new String('*', cardNumber.Length - firstDigits.Length - lastDigits.Length);

            return string.Concat(firstDigits, requiredMask, lastDigits);
        }
    }
}
