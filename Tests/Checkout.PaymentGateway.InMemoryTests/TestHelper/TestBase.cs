using System.Net.Http;
using Checkout.PaymentGateway.Host;
using Checkout.PaymentGateway.Host.AcquiringBankHandler;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;

namespace Checkout.PaymentGateway.InMemoryTests
{
    public abstract class TestBase
    {
        private TestServer _testServer;

        protected HttpClient _client;
        protected Mock<IAcquirerHandler> _bankHandlerMock;
        protected PaymentServiceClient _paymentServiceClient;

        [SetUp]
        public void SetUp()
        {
            var builder = new WebHostBuilder()
                .UseEnvironment("Development")
                .UseStartup<Startup>()
                .ConfigureTestServices(ConfigureOverrides);

            _testServer = new TestServer(builder);
            _client = _testServer.CreateClient();

            _paymentServiceClient = new PaymentServiceClient(_client);
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _client.Dispose();
            _testServer.Dispose();
        }

        private void ConfigureOverrides(IServiceCollection services)
        {
            _bankHandlerMock = new Mock<IAcquirerHandler>();

            services.AddSingleton(_bankHandlerMock.Object);
        }
    }
}