using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using Checkout.PaymentGateway.Host.Models;

namespace Checkout.PaymentGateway.Host.Repositories
{
    //just for show case, not for production
    public class InMemoryRepository<T> : IRepository<T> where T : IIdentifiable
    {
        private readonly ConcurrentDictionary<string, T> _checkoutMerchantDatabase = new ConcurrentDictionary<string, T>();

        public Task<T> GetAsync(IIdentifiable identifier)
        {
            if (identifier.MerchantAccount != "CheckoutCom")
            {
                throw new NotSupportedException();
            }

            if (_checkoutMerchantDatabase.TryGetValue(identifier.Id, out var value))
            {
                return Task.FromResult(value);
            }
            return Task.FromResult(default(T));
        }

        public Task SaveAsync(T data)
        {
            if (data.MerchantAccount != "CheckoutCom")
            {
                throw new NotSupportedException();
            }

            _checkoutMerchantDatabase.TryAdd(data.Id, data);

            return Task.FromResult(0);
        }
    }
}