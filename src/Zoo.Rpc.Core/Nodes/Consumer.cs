using System;
using System.Linq;
using Castle.DynamicProxy;
using Zoo.Rpc.Abstractions.Factories;
using Zoo.Rpc.Abstractions.LoadBalancers;
using Zoo.Rpc.Abstractions.Nodes;
using Zoo.Rpc.Core.Factories;
using Zoo.Rpc.Core.Interceptors;
using Zoo.Rpc.Core.Invokers;
using Zoo.Rpc.Core.Utils;

namespace Zoo.Rpc.Core.Nodes
{
    /// <summary>
    /// Consumer.
    /// </summary>
    public class Consumer : IRpcConsumer
    {
        private static readonly IProxyGenerator ProxyGenerator = new ProxyGenerator();

        private static readonly IRpcInvokerFactory InvokerFactory = new InvokerFactory();

        private static readonly IRpcLoadBalancerFactory LoadBalancerFactory = new LoadBalancerFactory();

        private readonly IRpcRegistry _registry;

        private readonly Type _serviceType;

        private readonly Uri _subscribingUri;

        private readonly IRpcLoadBalancer _rpcLoadBalancer;

        private readonly object _proxy;
        
        private IRpcInvoker _invoker;

        public Consumer(IRpcRegistry registry, Type serviceType, Uri serviceUri)
        {
            Uri = UriUtils.CreateConsumerUri(serviceType, serviceUri);
            
            _registry = registry;
            _serviceType = serviceType;
            _subscribingUri = UriUtils.CreateProviderUri(serviceType, serviceUri);
            _rpcLoadBalancer = LoadBalancerFactory.Create(serviceType, serviceUri);
            _proxy = MakeProxy();

            _invoker = new UnavailableInvoker(Uri, true);
        }
        
        public Uri Uri { get; }

        private object MakeProxy() =>
            ProxyGenerator.CreateInterfaceProxyWithoutTarget(_serviceType,
                new ProxyInterceptor(_serviceType, GetCurrentInvoker));

        private IRpcInvoker GetCurrentInvoker()
        {
            lock (_invoker)
            {
                return _invoker;
            }
        }

        private void Notify(Uri[] uris)
        {
            // Retrieve remote invokers by uris.
            var invokers = uris.Select(uri => InvokerFactory.Create(_serviceType, uri)).ToArray();
            
            // Construct invoker with load balancer and handlers.
            lock (_invoker)
            {
                _invoker = HandledInvoker.For(new LoadBalancedInvoker(this, _rpcLoadBalancer, invokers));
            }
        }

        public void Start()
        {
            // Register consumer node.
            _registry.Register(Uri);
            
            // Initial lookup.
            Notify(_registry.Lookup(_subscribingUri));
            
            // Subscribe changes of referenced provider nodes.
            _registry.Subscribe(_subscribingUri, Notify);
        }

        public object GetReference()
        {
            return _proxy;
        }
        
        public void Dispose()
        {
            // Unsubscribe referenced provider nodes.
            _registry.Unsubscribe(_subscribingUri, Notify);
            
            // Unregister consumer node.
            _registry.Unregister(Uri);
        }
    }
}