using System.Linq;
using AutoFixture;
using Checkout.PaymentGateway.Host.Mappers;
using Checkout.PaymentGateway.Host.Models;
using NUnit.Framework;

namespace Checkout.PaymentGateway.Host.UnitTests.Mappers
{
    [TestFixture]
    public class ProcessorResponseMapperTests
    {
        private ProcessorResponseMapper _processorResponseMapper;
        private Payment _payment;

        [SetUp]
        public void SetUp()
        {
            var fixture = new Fixture();
            _payment = fixture.Create<Payment>();

            _processorResponseMapper = new ProcessorResponseMapper();
        }

        [Test]
        public void Map_success()
        {
            _payment.AcquirerStatus = "Accepted";

            var result = _processorResponseMapper.Map(_payment);

            Assert.That(result.Payment, Is.EqualTo(_payment));
            Assert.IsNull(result.Errors);
        }

        [Test]
        public void Map_failed()
        {
            _payment.AcquirerStatus = "Failed";

            var result = _processorResponseMapper.Map(_payment);

            Assert.That(result.Payment, Is.EqualTo(_payment));
            Assert.That(result.Errors.Single().Code, Is.EqualTo("TakePaymentFailed"));
        }
    }
}