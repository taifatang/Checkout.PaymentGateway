using System.Threading.Tasks;
using Checkout.PaymentGateway.Host.Models;

namespace Checkout.PaymentGateway.Host.Repositories
{
    public interface IRepository<T>
    {
        Task<T> GetAsync(IIdentifiable id);
        Task SaveAsync(T data);
    }
}
