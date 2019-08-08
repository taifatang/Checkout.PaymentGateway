using Checkout.PaymentGateway.Host.Mappers;
using Checkout.PaymentGateway.Host.PaymentHandler;
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