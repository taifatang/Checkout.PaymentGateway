using System;
using System.Net.Http;
using Checkout.PaymentGateway.Host;
using Checkout.PaymentGateway.Host.AcquiringBanks;
using Checkout.PaymentGateway.InMemoryTests.Stubs;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;

namespace Checkout.PaymentGateway.InMemoryTests.TestHelper
{
    public class InMemoryHost : IDisposable
    {
        private TestServer _testServer;

        public HttpClient _client;
        public IServiceProvider _services;

        public InMemoryHost()
        {
            var builder = new WebHostBuilder()
                .UseKestrel()
                .UseEnvironment("Development")
                .UseStartup<Startup>()
                .ConfigureTestServices(ConfigureOverrides);

            _testServer = new TestServer(builder);

            _client = _testServer.CreateClient();
            _services = _testServer.Host.Services;
        }

        private void ConfigureOverrides(IServiceCollection services)
        {
            services.AddTransient<IAcquirerHandler, BankStub>();
        }

        public void Dispose()
        {
            _client.Dispose();
            _testServer.Dispose();
        }
    }
}