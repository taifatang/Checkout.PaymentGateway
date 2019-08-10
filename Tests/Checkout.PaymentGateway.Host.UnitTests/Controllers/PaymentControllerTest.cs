using System.Net;
using System.Threading.Tasks;
using AutoFixture;
using Checkout.PaymentGateway.Host.Contracts;
using Checkout.PaymentGateway.Host.Controllers;
using Checkout.PaymentGateway.Host.Mappers;
using Checkout.PaymentGateway.Host.Models;
using Checkout.PaymentGateway.Host.PaymentProcessor;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;

namespace Checkout.PaymentGateway.Host.UnitTests.Controllers
{
    [TestFixture]
    public class PaymentControllerTest
    {
        private PaymentController _paymentController;
        private Mock<IPaymentProcessor<AuthoriseRequest>> _paymentProcessor;
        private Mock<IMapper> _mapperMock;

        [SetUp]
        public void SetUp()
        {
            var processorFactoryMock = new Mock<IPaymentProcessorFactory>();

            _mapperMock = new Mock<IMapper>();

            _paymentProcessor = new Mock<IPaymentProcessor<AuthoriseRequest>>();
            processorFactoryMock.Setup(x => x.Get<AuthoriseRequest>()).Returns(_paymentProcessor.Object);

            _paymentController = new PaymentController(processorFactoryMock.Object, _mapperMock.Object);
        }

        [Test]
        public async Task Returns_422_status_code_when_authorise_failed()
        {
            var authoriseResponse = new Fixture()
                .Build<AuthoriseResponse>()
                .With(x => x.Status, PaymentStatus.Failed)
                .Create();
            _mapperMock.Setup(x => x.Map<ProcessorResponse, AuthoriseResponse>(It.IsAny<ProcessorResponse>()))
                .Returns(authoriseResponse);

            var response = (UnprocessableEntityObjectResult)await _paymentController.Authorise(new AuthoriseRequest());

            Assert.That(response.StatusCode, Is.EqualTo((int)HttpStatusCode.UnprocessableEntity));
            Assert.That(response.Value, Is.EqualTo(authoriseResponse));
            _paymentProcessor.VerifyAll();
            _mapperMock.VerifyAll();
        }

        [Test]
        public async Task Returns_200_status_code_when_authorise_success()
        {
            var authoriseResponse = new Fixture()
                .Build<AuthoriseResponse>()
                .With(x => x.Status, PaymentStatus.Accepted)
                .Create();
            _mapperMock.Setup(x => x.Map<ProcessorResponse, AuthoriseResponse>(It.IsAny<ProcessorResponse>()))
                .Returns(authoriseResponse);

            var response = (OkObjectResult)await _paymentController.Authorise(new AuthoriseRequest());

            Assert.That(response.StatusCode, Is.EqualTo(200));
            Assert.That(response.Value, Is.EqualTo(authoriseResponse));
            _paymentProcessor.VerifyAll();
            _mapperMock.VerifyAll();
        }

    }
}
