using System;
using Microsoft.Extensions.DependencyInjection;

namespace Checkout.PaymentGateway.Host.Mappers
{
    public class Mapper : IMapper
    {
        private readonly IServiceProvider _serviceProvider;

        public Mapper(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public TTo Map<TFrom, TTo>(TFrom from)
        {
            var mapper = _serviceProvider.GetRequiredService<IMap<TFrom, TTo>>();

            return mapper.Map(from);
        }
    }
}