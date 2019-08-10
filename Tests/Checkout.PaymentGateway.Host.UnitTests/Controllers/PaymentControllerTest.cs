using System.Net;
using System.Threading.Tasks;
using AutoFixture;
using Checkout.PaymentGateway.Host.Contracts;
using Checkout.PaymentGateway.Host.Controllers;
using Checkout.PaymentGateway.Host.Mappers;
using Checkout.PaymentGateway.Host.Models;
using Checkout.PaymentGateway.Host.PaymentProcessor;
using Checkout.PaymentGateway.Host.Repositories;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;

namespace Checkout.PaymentGateway.Host.UnitTests.Controllers
{
    [TestFixture]
    public class PaymentControllerTest
    {
        private PaymentController _paymentController;
        private Mock<IPaymentProcessor<AuthoriseRequest>> _paymentProcessorMock;
        private Mock<IMapper> _mapperMock;
        private Mock<IRepository<Payment>> _repositoryMock;
        private Fixture _fixture;

        [SetUp]
        public void SetUp()
        {
            var processorFactoryMock = new Mock<IPaymentProcessorFactory>();
            _fixture = new Fixture();
            _mapperMock = new Mock<IMapper>();
            _repositoryMock = new Mock<IRepository<Payment>>();

            _paymentProcessorMock = new Mock<IPaymentProcessor<AuthoriseRequest>>();
            processorFactoryMock.Setup(x => x.Get<AuthoriseRequest>()).Returns(_paymentProcessorMock.Object);

            _paymentController = new PaymentController(processorFactoryMock.Object, _mapperMock.Object, _repositoryMock.Object);
        }

        [Test]
        public async Task Returns_422_status_code_when_authorise_failed()
        {
            var authoriseResponse = _fixture
                .Build<AuthoriseResponse>()
                .With(x => x.Status, PaymentStatus.Failed)
                .Create();
            _mapperMock.Setup(x => x.Map<ProcessorResponse, AuthoriseResponse>(It.IsAny<ProcessorResponse>()))
                .Returns(authoriseResponse);

            var response = (UnprocessableEntityObjectResult)await _paymentController.Authorise(new AuthoriseRequest());

            Assert.That(response.StatusCode, Is.EqualTo((int)HttpStatusCode.UnprocessableEntity));
            Assert.That(response.Value, Is.EqualTo(authoriseResponse));
            _paymentProcessorMock.VerifyAll();
            _mapperMock.VerifyAll();
        }

        [Test]
        public async Task Returns_200_status_code_when_authorise_is_successful()
        {
            var authoriseResponse = _fixture
                .Build<AuthoriseResponse>()
                .With(x => x.Status, PaymentStatus.Accepted)
                .Create();
            _mapperMock.Setup(x => x.Map<ProcessorResponse, AuthoriseResponse>(It.IsAny<ProcessorResponse>()))
                .Returns(authoriseResponse);

            var response = (OkObjectResult)await _paymentController.Authorise(new AuthoriseRequest());

            Assert.That(response.StatusCode, Is.EqualTo((int)HttpStatusCode.OK));
            Assert.That(response.Value, Is.EqualTo(authoriseResponse));
            _paymentProcessorMock.VerifyAll();
            _mapperMock.VerifyAll();
        }

        [Test]
        public async Task Returns_404_status_code_when_payment_is_not_found()
        {
            _repositoryMock.Setup(x => x.GetAsync(It.IsAny<GetPaymentRequest>())).Returns(Task.FromResult<Payment>(null));

            var response = (NotFoundResult)await _paymentController.Get("", "");

            Assert.That(response.StatusCode, Is.EqualTo((int)HttpStatusCode.NotFound));
            _repositoryMock.VerifyAll();
        }

        [Test]
        public async Task Returns_200_status_code_when_payment_is_retrieved()
        {
            var payment = _fixture.Create<Payment>();
            _repositoryMock.Setup(x => x.GetAsync(It.IsAny<GetPaymentRequest>())).ReturnsAsync(payment);

            var response = (OkObjectResult)await _paymentController.Get("", "");

            Assert.That(response.StatusCode, Is.EqualTo((int)HttpStatusCode.OK));
            Assert.That(response.Value, Is.EqualTo(payment));
            _repositoryMock.VerifyAll();
        }
    }
}
