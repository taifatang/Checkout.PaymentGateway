using System.Threading.Tasks;
using Checkout.PaymentGateway.Host.Contracts.Acquirers;

namespace Checkout.PaymentGateway.Host.AcquiringBanks
{
    //Not a requirement - New Relic monitoring
    //New Relic already capture a lot of information including endpoint, response, duration etc. 
    public class MonitorAcquirerHandler : IAcquirerHandler
    {
        private readonly IAcquirerHandler _inner;
        //private readonly NewRelicMonitor _monitor;
        public MonitorAcquirerHandler(IAcquirerHandler inner)
        {
            _inner = inner;
        }

        public async Task<AcquirerResponse> ProcessAsync(AcquirerRequest request)
        {
            //_monitor.AddParameter(request.PaymentOperation);
            //_monitor.AddParameter(request.DesignatedAccount);
            //_monitor.AddParameter(request.CurrencyCode);
            //_monitor.AddParameter(request.Amount);
            //_monitor.AddParameter(request.CardDetails.CardNumber);
            //_monitor.AddParameter(request.CardDetails.ExpiryDate);

            AcquirerResponse acquirerResponse;

            //using (_monitor.Timer(nameof(IAcquirerHandler)))
            //{
            acquirerResponse = await _inner.ProcessAsync(request);
            //}

            //_monitor.AddParameter(_acquirerResponse.Id);
            //_monitor.AddParameter(_acquirerResponse.Status);

            return acquirerResponse;
        }
    }
}