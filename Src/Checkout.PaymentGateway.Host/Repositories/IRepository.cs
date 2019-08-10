using System.Threading.Tasks;

namespace Checkout.PaymentGateway.Host.Repositories
{
    public interface IRepository<T>
    {
        T GetAsync(string id);
        Task SaveAsync(T data);
    }
}
