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
                PaymentDetails = new PaymentDetails()
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
        }

        [Test]
        public async Task Payment_authorisation_failed()
        {
            var expectedRejectionRequest = new AuthoriseRequest()
            {
                PaymentDetails = new PaymentDetails()
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
        }

        [Test]
        public async Task Payment_authorisation_with_invalid_payment()
        {
            var invalidRequest = new AuthoriseRequest();

            var response = await PaymentServiceClient.Authorise(invalidRequest);

            Assert.That(response.HttpStatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            Assert.IsNotEmpty(response.Content.Errors);
        }
    }
}
