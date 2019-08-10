using System;
using System.Net;
using System.Threading.Tasks;
using Checkout.PaymentGateway.InMemoryTests.Contracts;
using Checkout.PaymentGateway.InMemoryTests.TestHelper;
using NUnit.Framework;

namespace Checkout.PaymentGateway.InMemoryTests.Features
{
    [TestFixture]
    public class GetPaymentTests : TestBase
    {
        [Test]
        public async Task Retrieve_an_existing_payment()
        {
            var request = new AuthoriseRequest()
            {
                CardDetails = new CardDetails()
                {
                    CardNumber = "424242424242",
                    ExpiryDate = "0129",
                    SecurityCode = "111"
                },
                MerchantAccount = "CheckoutCom",
                Amount = 25.00m,
                CurrencyCode = "GBP"
            };

            var authoriseResponse = await PaymentServiceClient.Authorise(request);

            var paymentResponse = await PaymentServiceClient.GetPayment(authoriseResponse.Content.Id, "CheckoutCom");

            Assert.That(paymentResponse.HttpStatusCode, Is.EqualTo(HttpStatusCode.OK));

            Assert.That(paymentResponse.Content.Id, Is.EqualTo(paymentResponse.Content.Id));
            Assert.That(paymentResponse.Content.Amount, Is.EqualTo(paymentResponse.Content.Amount));
            Assert.That(paymentResponse.Content.CurrencyCode, Is.EqualTo(paymentResponse.Content.CurrencyCode));
            Assert.That(paymentResponse.Content.Status, Is.EqualTo(paymentResponse.Content.Status));
            Assert.That(paymentResponse.Content.CardDetails.CardNumber, Is.EqualTo(paymentResponse.Content.CardDetails.CardNumber));
            Assert.That(paymentResponse.Content.CardDetails.ExpiryDate, Is.EqualTo(paymentResponse.Content.CardDetails.ExpiryDate));
            Assert.That(paymentResponse.Content.CardDetails.SecurityCode, Is.EqualTo(paymentResponse.Content.CardDetails.SecurityCode));
        }
        [Test]
        public async Task Payment_not_found()
        {
            var paymentResponse = await PaymentServiceClient.GetPayment(Guid.NewGuid().ToString(), "CheckoutCom");

            Assert.That(paymentResponse.HttpStatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }
    }
}
