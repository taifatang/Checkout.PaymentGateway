namespace Checkout.PaymentGateway.Host.Mappers
{
    public interface IMap<TFrom, TTo>
    {
        TTo Map(TFrom from);
    }
}