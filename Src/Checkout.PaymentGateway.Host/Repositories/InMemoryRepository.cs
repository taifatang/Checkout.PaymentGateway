using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using Checkout.PaymentGateway.Host.Models;

namespace Checkout.PaymentGateway.Host.Repositories
{
    //just for show case, not for production
    public class InMemoryRepository<T> : IRepository<T> where T : IIdentifiable
    {
        private ConcurrentDictionary<string, T> _dictionary = new ConcurrentDictionary<string, T>();

        public T GetAsync(string id)
        {
            if (_dictionary.TryGetValue(id, out var value))
            {
                return value;
            }
            throw new InvalidOperationException();
        }

        public Task SaveAsync(T data)
        {
            if (_dictionary.TryAdd(data.Id, data))
            {
                return Task.FromResult(0);
            }
            throw new InvalidOperationException();
        }
    }
}