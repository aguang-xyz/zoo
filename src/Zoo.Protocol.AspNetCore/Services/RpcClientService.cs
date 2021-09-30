using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Zoo.Protocol.AspNetCore.Nodes;
using Zoo.Rpc.Client;

namespace Zoo.Protocol.AspNetCore.Services
{
    internal sealed class RpcClientService : IHostedService
    {
        public static readonly IList<Type> ConsumerTypes = new List<Type>();
        
        public static readonly IList<Type> ProviderTypes = new List<Type>();

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

        public TService Consume<TService>() where TService : class
        {
            return _client.Consume(typeof(TService)) as TService;
        }

        public void Start()
        {
            foreach (var consumerType in ConsumerTypes)
            {
                _client.Consume(consumerType);
            }
                
            foreach (var providerType in ProviderTypes)
            {
                _client.Provide(providerType, new AspNetCoreInvoker(providerType, _serviceProvider));
            }

            _client.Start();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            return Task.Run(Start, cancellationToken);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.Run(() => _client.Dispose(), cancellationToken);
        }
    }
}