using AutoFixture;
using Checkout.PaymentGateway.Host.Contracts;
using Checkout.PaymentGateway.Host.Contracts.Acquirers;
using Checkout.PaymentGateway.Host.Mappers;
using NUnit.Framework;

namespace Checkout.PaymentGateway.Host.UnitTests.Mappers
{
    [TestFixture]
    public class AcquirerRequestMapperTests
    {
        private AcquirerRequestMapper _acquirerRequestMapper;
        private AuthoriseRequest _authoriseRequest;

        [SetUp]
        public void SetUp()
        {
            var fixture = new Fixture();
            _authoriseRequest = fixture.Create<AuthoriseRequest>();

            _acquirerRequestMapper = new AcquirerRequestMapper();
        }
        [Test]
        public void Map()
        {
            var result = _acquirerRequestMapper.Map(_authoriseRequest);

            Assert.That(result.PaymentOperation, Is.EqualTo(PaymentOperation.Authorise));
            Assert.That(result.DesignatedAccount, Is.EqualTo(_authoriseRequest.MerchantAccount));
            Assert.That(result.CardDetails, Is.EqualTo(_authoriseRequest.CardDetails));
            Assert.That(result.Amount, Is.EqualTo(_authoriseRequest.Amount));
            Assert.That(result.CurrencyCode, Is.EqualTo(_authoriseRequest.CurrencyCode));
        }
    }
}