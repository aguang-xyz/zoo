using System;
using System.Collections.Generic;
using Zoo.Rpc.Abstractions.Constants;
using Zoo.Rpc.Abstractions.Extensions;
using Zoo.Rpc.Abstractions.Factories;
using Zoo.Rpc.Abstractions.Nodes;
using Zoo.Rpc.Core.Factories;

namespace Zoo.Rpc.Client
{
    /// <summary>
    /// Rpc client.
    /// </summary>
    public class RpcClient : IRpcClient
    {
        private static readonly IRpcRegistryFactory RegistryFactory = new RegistryFactory();

        private static readonly IRpcProviderFactory ProviderFactory = new ProviderFactory();
        
        private static readonly IRpcConsumerFactory ConsumerFactory = new ConsumerFactory();

        private readonly RpcClientOptions _options;

        private readonly IRpcRegistry _registry;

        private readonly IList<IRpcProvider> _providers = new List<IRpcProvider>();

        private readonly IList<IRpcConsumer> _consumers = new List<IRpcConsumer>();

        private bool _started;

        public RpcClient(RpcClientOptions options)
        {
            _options = options;
            _registry = RegistryFactory.Create(options.RegistryUri);
        }

        public void Provide(Type serviceType, IRpcInvoker invoker)
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
                _providers.Add(ProviderFactory.Create(_registry, serviceType, _options.ServiceUri, invoker));
            }
        }

        public object Consume(Type serviceType)
        {
            lock (this)
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
            lock (this)
            {
                foreach (var provider in _providers)
                {
                    provider.Dispose();
                }

                foreach (var consumer in _consumers)
                {
                    consumer.Dispose();
                }
            }

            _registry.Dispose();
        }
    }
}