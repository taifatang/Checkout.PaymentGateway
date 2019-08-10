using System.Threading.Tasks;
using AutoFixture;
using Checkout.PaymentGateway.Host.AcquiringBank;
using Checkout.PaymentGateway.Host.Contracts;
using Checkout.PaymentGateway.Host.Contracts.Acquirers;
using Checkout.PaymentGateway.Host.Mappers;
using Checkout.PaymentGateway.Host.Models;
using Checkout.PaymentGateway.Host.PaymentProcessor;
using Checkout.PaymentGateway.Host.Repositories;
using Moq;
using NUnit.Framework;
using CardDetails = Checkout.PaymentGateway.Host.Models.CardDetails;

namespace Checkout.PaymentGateway.Host.UnitTests.Processors
{
    [TestFixture]
    public class PaymentAuthorisationProcessorTests
    {
        private PaymentAuthorisationProcessor _authorisationProcessor;
        private Mock<IRepository<Payment>> _repository;
        private Mock<IAcquirerHandler> _acquirerHandlerMock;
        private Mock<ICardDetailsMasker> _cardDetailsMaskerMock;
        private Mock<IMapper> _mapperMock;

        private Fixture _fixture;
        private AuthoriseRequest _authoriseRequest;
        private AcquirerRequest _acquirerRequest;
        private AcquirerResponse _acquirerResponse;

        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture();
            _acquirerRequest = _fixture.Create<AcquirerRequest>();
            _acquirerResponse = _fixture.Create<AcquirerResponse>();
            _authoriseRequest = _fixture.Create<AuthoriseRequest>();
            _authoriseRequest.CardDetails.CardNumber = "4242 4242 4242 4242";

            _repository = new Mock<IRepository<Payment>>();
            _acquirerHandlerMock = new Mock<IAcquirerHandler>();
            _cardDetailsMaskerMock = new Mock<ICardDetailsMasker>();
            _mapperMock = new Mock<IMapper>();

            _authorisationProcessor = new PaymentAuthorisationProcessor(_repository.Object, _acquirerHandlerMock.Object, _cardDetailsMaskerMock.Object, _mapperMock.Object);

            _mapperMock.Setup(x => x.Map<AuthoriseRequest, AcquirerRequest>(It.IsAny<AuthoriseRequest>())).Returns(_acquirerRequest);
            _acquirerHandlerMock.Setup(x => x.ProcessAsync(_acquirerRequest)).ReturnsAsync(_acquirerResponse);
        }

        [Test]
        public async Task Process_payment()
        {
            await _authorisationProcessor.ExecuteAsync(_authoriseRequest);

            _acquirerHandlerMock.Verify(x => x.ProcessAsync(_acquirerRequest), Times.Once);
        }

        [Test]
        public async Task Mask_payment()
        {
            await _authorisationProcessor.ExecuteAsync(_authoriseRequest);

            _acquirerHandlerMock.Verify(x => x.ProcessAsync(_acquirerRequest), Times.Once);
        }

        [Test]
        public async Task Save_Payment_result_with_masked_card_details()
        {
            var cardDetails = _fixture.Create<CardDetails>();
            _cardDetailsMaskerMock.Setup(x => x.Mask(It.IsAny<CardDetails>())).Returns(cardDetails);

            await _authorisationProcessor.ExecuteAsync(_authoriseRequest);

            _repository.Verify(x => x.SaveAsync(It.Is<Payment>(p => IsExpectedPayment(p, cardDetails))), Times.Once);
        }

        [TearDown]
        public void TearDown()
        {
            _acquirerHandlerMock.VerifyAll();
            _repository.VerifyAll();
            _mapperMock.VerifyAll();
        }

        private bool IsExpectedPayment(Payment payment, CardDetails cardDetails)
        {
            Assert.IsNotNull(payment.Id);

            Assert.That(payment.AcquirerReference, Is.EqualTo(_acquirerResponse.Id));
            Assert.That(payment.AcquirerStatus, Is.EqualTo(_acquirerResponse.Status));
            Assert.That(payment.Amount, Is.EqualTo(_authoriseRequest.Amount));
            Assert.That(payment.Currency, Is.EqualTo(_authoriseRequest.CurrencyCode));
            Assert.That(payment.CardDetails, Is.EqualTo(cardDetails));

            return true;
        }
    }
}
