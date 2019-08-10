using Checkout.PaymentGateway.Host.Mappers;
using Checkout.PaymentGateway.Host.Models;
using NUnit.Framework;

namespace Checkout.PaymentGateway.Host.UnitTests.Mappers
{
    [TestFixture]
    public class CardDetailsMaskerTests
    {
        [TestCase("424242424242", "424242**4242")]
        [TestCase("4242424242424", "424242***2424")]
        [TestCase("42424242424242", "424242****4242")]
        [TestCase("424242424242424", "424242*****2424")]
        [TestCase("4242424242424242", "424242******4242")]
        [TestCase("42424242424242424", "424242*******2424")]
        [TestCase("424242424242424242", "424242********4242")]
        [TestCase("4242424242424242424", "424242*********2424")]
        [TestCase("abcdefghijkl", "abcdef**ijkl")]
        [TestCase(null, null)]
        [TestCase("", "")]
        public void Mask_card_number(string cardNumber, string expectedMaskedCardNumber)
        {
            var cardDetails = new CardDetails { CardNumber = cardNumber };

            var result = new CardDetailsMasker().Mask(cardDetails);

            Assert.That(result.CardNumber, Is.EqualTo(expectedMaskedCardNumber));
        }

        [TestCase("123")]
        [TestCase("1234")]
        [TestCase("")]
        [TestCase(null)]
        public void Nullify_card_security_code(string securityCode)
        {
            var cardDetails = new CardDetails { SecurityCode = securityCode };

            var result = new CardDetailsMasker().Mask(cardDetails);

            Assert.IsNull(result.CardNumber);
        }
    }
}