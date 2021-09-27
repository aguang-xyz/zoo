using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Zoo.Protocol.AspNetCore.Services;

namespace Zoo.Protocol.AspNetCore.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddRpcClientService(this IServiceCollection services)
        {
            return services.AddHostedService<RpcClientService>();
        }

        public static IServiceCollection AddConsumer<TService>(this IServiceCollection services) where TService : class
        {
            return services.AddScoped(serviceProvider =>
            {
                var clientService = (RpcClientService) serviceProvider
                    .GetServices<IHostedService>()
                    .First(service => service.GetType() == typeof(RpcClientService));
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