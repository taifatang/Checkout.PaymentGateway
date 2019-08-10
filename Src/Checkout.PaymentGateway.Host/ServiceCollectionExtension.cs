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
            RegisterRepository(services);
            RegisterAcquirers(services);
        }

        private static void RegisterMappers(IServiceCollection services)
        {
            services.AddSingleton<IMapper, Mapper>();
            services.AddSingleton<ICardDetailsMasker, CardDetailsMasker>();

            services.Scan(scan => scan.FromAssembliesOf(typeof(IMap<,>))
                .AddClasses(classes => classes.AssignableTo(typeof(IMap<,>)))
                .AsImplementedInterfaces()
                .WithSingletonLifetime());
        }

        public static void RegisterPaymentProcessors(IServiceCollection services)
        {
            services.AddSingleton<IPaymentProcessorFactory, PaymentProcessorFactory>();

            services.Scan(scan => scan.FromAssembliesOf(typeof(IPaymentProcessor<>))
                .AddClasses(classes => classes.AssignableTo(typeof(IPaymentProcessor<>)))
                .AsImplementedInterfaces()
                .WithTransientLifetime());
        }

        public static void RegisterRepository(IServiceCollection services)
        {
            services.AddSingleton(typeof(IRepository<>), typeof(InMemoryRepository<>));

            //services.AddSingleton(typeof(IRepository<>), typeof(InMemoryRepository<>));
            //services.Decorate(typeof(IRepository<>), typeof(IEncyptionRepository<>));
        }

        public static void RegisterAcquirers(IServiceCollection services)
        {
            services.AddTransient<IAcquirerHandler, FakeBarclaysBankAcquirerHandler>();
            services.Decorate<IAcquirerHandler, MonitorAcquirerHandler>();
        }
    }
}