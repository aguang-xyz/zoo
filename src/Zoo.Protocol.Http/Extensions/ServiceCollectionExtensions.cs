using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Zoo.Protocol.Http.Services;

namespace Zoo.Protocol.Http.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddRpcClientService(this IServiceCollection services)
        {
            return services.AddHostedService<RpcClientService>();
        }

        public static IServiceCollection AddConsumer<TService>(this IServiceCollection services) where TService : class
        {
            RpcClientService.ConsumerTypes.Add(typeof(TService));
            return services.AddScoped(serviceProvider =>
            {
                var clientService = (RpcClientService) serviceProvider
                    .GetServices<IHostedService>()
                    .First(service => service is RpcClientService);
                return clientService.Consume<TService>();
            });
        }

        public static IServiceCollection AddProvider<TService>(this IServiceCollection services)
        {
            RpcClientService.ProviderTypes.Add(typeof(TService));
            return services;
        }
    }
}