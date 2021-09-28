using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Castle.DynamicProxy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Zoo.Protocol.AspNetCore.Interceptors;
using Zoo.Rpc.Client;

namespace Zoo.Protocol.AspNetCore.Services
{
    internal sealed class RpcClientService : IHostedService
    {
        public static readonly IList<Type> ConsumerTypes = new List<Type>();
        
        public static readonly IList<Type> ProviderTypes = new List<Type>();

        private static readonly IProxyGenerator ProxyGenerator = new ProxyGenerator();
        
        private readonly IServiceProvider _serviceProvider;

        private readonly IRpcClient _client;
        
        public RpcClientService(IServiceProvider serviceProvider, IConfiguration configuration)
        {
            _serviceProvider = serviceProvider;
            _client = new DefaultRpcClient(new RpcClientOptions
            {
                RegistryUri = new Uri(configuration["Zoo:RegistryUri"]),
                ServiceUri = new Uri(configuration["Zoo:ServiceUri"])
            });
        }

        public TService Consume<TService>()
        {
            return _client.Consume<TService>();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            return Task.Run(() =>
            {
                foreach (var consumerType in ConsumerTypes)
                {
                    _client.Consume(consumerType);
                }
                
                foreach (var providerType in ProviderTypes)
                {
                    var interceptor = new AspNetCoreRpcInterceptor(_serviceProvider, providerType);
                    var proxy = ProxyGenerator.CreateInterfaceProxyWithoutTarget(providerType, interceptor);

                    _client.Provide(providerType, proxy);
                }

                _client.Start();
            }, cancellationToken);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.Run(() =>
            {
                _client.Dispose();
            }, cancellationToken);
        }
    }
}