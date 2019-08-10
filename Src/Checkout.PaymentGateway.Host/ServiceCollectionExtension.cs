using Checkout.PaymentGateway.Host.AcquiringBanks;
using Checkout.PaymentGateway.Host.Mappers;
using Checkout.PaymentGateway.Host.PaymentProcessor;
using Checkout.PaymentGateway.Host.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Checkout.PaymentGateway.Host
{
    public static class ServiceCollectionExtension
    {
        public static void RegisterDependencies(this IServiceCollection services)
        {
            RegisterMappers(services);
            RegisterPaymentProcessors(services);
        }

        private static void RegisterMappers(IServiceCollection services)
        {
            services.AddSingleton<IMapper, Mapper>();
            services.AddSingleton<ICardDetailsMasker, CardDetailsMasker>();
            services.Scan(scan => scan.FromAssemblyOf<Startup>()
                .AddClasses(classes => classes.AssignableTo(typeof(IMap<,>)))
                .AsImplementedInterfaces()
                .WithSingletonLifetime());
        }

        public static void RegisterPaymentProcessors(IServiceCollection services)
        {
            services.AddSingleton<PaymentProcessorFactory>();
            services.Scan(scan => scan.FromAssemblyOf<Startup>()
                .AddClasses(classes => classes.AssignableTo(typeof(IPaymentProcessor<>)))
                .AsImplementedInterfaces()
                .WithSingletonLifetime());
        }
    }
}