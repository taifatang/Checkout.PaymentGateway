namespace Checkout.PaymentGateway.Host.Mappers
{
    public interface IMapper
    {
        TTo Map<TFrom, TTo>(TFrom from);
    }
}