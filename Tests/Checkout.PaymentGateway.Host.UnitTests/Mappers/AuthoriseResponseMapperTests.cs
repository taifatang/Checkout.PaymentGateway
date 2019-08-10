using AutoFixture;
using Checkout.PaymentGateway.Host.Mappers;
using Checkout.PaymentGateway.Host.Models;
using NUnit.Framework;

namespace Checkout.PaymentGateway.Host.UnitTests.Mappers
{
    [TestFixture]
    public class AuthoriseResponseMapperTests
    {
        private AuthoriseResponseMapper _authoriseResponseMapper;
        private ProcessorResponse _processorResponse;

        [SetUp]
        public void SetUp()
        {
            var fixture = new Fixture();
            _processorResponse = fixture.Create<ProcessorResponse>();

            _authoriseResponseMapper = new AuthoriseResponseMapper();
        }

        [Test]
        public void Map()
        {
            var result = _authoriseResponseMapper.Map(_processorResponse);

            Assert.That(result.Id, Is.EqualTo(_processorResponse.Payment.Id));
            Assert.That(result.Amount, Is.EqualTo(_processorResponse.Payment.Amount));
            Assert.That(result.CurrencyCode, Is.EqualTo(_processorResponse.Payment.Currency));
            Assert.That(result.CardDetails.CardNumber, Is.EqualTo(_processorResponse.Payment.CardDetails.CardNumber));
            Assert.That(result.CardDetails.ExpiryDate, Is.EqualTo(_processorResponse.Payment.CardDetails.ExpiryDate));
            Assert.IsNull(result.CardDetails.SecurityCode);
        }

        [Test]
        public void Map_success_status()
        {
            _processorResponse.Payment.AcquirerStatus = "Accepted";

            var result = _authoriseResponseMapper.Map(_processorResponse);

            Assert.That(result.Status, Is.EqualTo(PaymentStatus.Accepted));
            Assert.IsNull(result.Errors);
        }

        [Test]
        public void Map_failed_status()
        {
            _processorResponse.Payment.AcquirerStatus = "Failed";

            var result = _authoriseResponseMapper.Map(_processorResponse);

            Assert.That(result.Status, Is.EqualTo(PaymentStatus.Failed));
            Assert.IsNotEmpty(result.Errors);
        }
    }
}
