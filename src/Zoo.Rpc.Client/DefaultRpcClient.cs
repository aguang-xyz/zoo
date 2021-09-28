using System;
using System.Collections.Generic;
using Zoo.Rpc.Abstractions.Constants;
using Zoo.Rpc.Abstractions.Factories;
using Zoo.Rpc.Abstractions.Nodes;
using Zoo.Rpc.Shared.Extensions;
using Zoo.Rpc.Shared.Factories;

namespace Zoo.Rpc.Client
{
    /// <summary>
    /// Default Rpc client.
    /// </summary>
    public class DefaultRpcClient : IRpcClient
    {
        private static readonly IRpcRegistryFactory RegistryFactory = new DefaultRpcRegistryFactory();

        private static readonly IRpcProviderFactory ProviderFactory = new DefaultRpcProviderFactory();
        
        private static readonly IRpcConsumerFactory ConsumerFactory = new DefaultRpcConsumerFactory();

        private readonly RpcClientOptions _options;

        private readonly IRpcRegistry _registry;

        private readonly IList<IRpcProvider> _providers = new List<IRpcProvider>();

        private readonly IList<IRpcConsumer> _consumers = new List<IRpcConsumer>();

        private bool _started;

        public DefaultRpcClient(RpcClientOptions options)
        {
            _options = options;
            _registry = RegistryFactory.Create(options.RegistryUri);
        }

        public void Provide<TService>(object service)
        {
            Provide(typeof(TService), service);
        }

        public void Provide(Type serviceType, object service)
        {
            lock (this)
            {
                // Try to void redundant providers.
                foreach (var p in _providers)
                {
                    if (p.Uri.GetServiceName() == serviceType.FullName &&
                        p.Uri.GetNodeType() == NodeTypes.Consumer)
                    {
                        return;
                    }
                }
                
                // Add a new provider node.
                _providers.Add(ProviderFactory.Create(_registry, serviceType, _options.ServiceUri, service));
            }
        }

        public TService Consume<TService>()
        {
            return (TService) Consume(typeof(TService));
        }

        public object Consume(Type serviceType)
        {
            lock (_consumers)
            {
                // Try to reuse existed consumer.
                foreach (var c in _consumers)
                {
                    if (c.Uri.GetServiceName() == serviceType.FullName && c.Uri.GetNodeType() == NodeTypes.Consumer)
                    {
                        return c.GetReference();
                    }
                }

                // Add a new consumer node.
                var consumer = ConsumerFactory.Create(_registry, serviceType, _options.ServiceUri);

                _consumers.Add(consumer);

                if (_started)
                {
                    consumer.Start();
                }
                
                return consumer.GetReference();
            }
        }

        public void Start()
        {
            lock (this)
            {
                foreach (var consumer in _consumers)
                {
                    consumer.Start();
                }
                
                foreach (var provider in _providers)
                {
                    provider.Start();
                }

                _started = true;
            }
        }
        
        public void Dispose()
        {
            lock (_providers)
            {
                foreach (var provider in _providers)
                {
                    provider.Dispose();
                }
            }
            
            lock (_consumers)
            {
                foreach (var consumer in _consumers)
                {
                    consumer.Dispose();
                }
            }

            _registry.Dispose();
        }
    }
}