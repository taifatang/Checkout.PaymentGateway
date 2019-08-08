using NUnit.Framework;

namespace Checkout.PaymentGateway.InMemoryTests.TestHelper
{
    public abstract class TestBase
    {
        protected InMemoryHost InMemoryHost;
        protected PaymentServiceClient PaymentServiceClient;

        [SetUp]
        public void SetUp()
        {
            InMemoryHost = new InMemoryHost();
            PaymentServiceClient = new PaymentServiceClient(InMemoryHost._client);
        }

        [TearDown]
        public void TearDown()
        {
            InMemoryHost.Dispose();
        }
    }
}