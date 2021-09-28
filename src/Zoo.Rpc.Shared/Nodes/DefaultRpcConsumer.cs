using System;
using System.Linq;
using Castle.DynamicProxy;
using Zoo.Rpc.Abstractions.Factories;
using Zoo.Rpc.Abstractions.LoadBalancers;
using Zoo.Rpc.Abstractions.Nodes;
using Zoo.Rpc.Shared.Extensions;
using Zoo.Rpc.Shared.Factories;
using Zoo.Rpc.Shared.Interceptors;
using Zoo.Rpc.Shared.Utils;

namespace Zoo.Rpc.Shared.Nodes
{
    /// <summary>
    /// Default consumer.
    /// </summary>
    public class DefaultRpcConsumer : IRpcConsumer
    {
        private static readonly IProxyGenerator ProxyGenerator = new ProxyGenerator();

        private static readonly IRpcInvokerFactory InvokerFactory = new DefaultRpcInvokerFactory();

        private static readonly IRpcLoadBalancerFactory LoadBalancerFactory = new DefaultRpcLoadBalancerFactory();

        private readonly IRpcRegistry _registry;

        private readonly Type _serviceType;

        private readonly Uri _subscribingUri;

        private readonly IRpcLoadBalancer _loadBalancer;

        private IRpcInvoker _invoker = new UnavailableRpcInvoker();

        private object _proxy;

        public DefaultRpcConsumer(IRpcRegistry registry, Type serviceType, Uri serviceUri)
        {
            _registry = registry;
            _serviceType = serviceType;
            _subscribingUri = UriUtils.CreateProviderUri(serviceType, serviceUri);
            _loadBalancer = LoadBalancerFactory.Create(serviceType, serviceUri);
            _proxy = MakeProxy();
            Uri = UriUtils.CreateConsumerUri(serviceType, serviceUri);
        }
        
        public Uri Uri { get; }

        private object MakeProxy() =>
            ProxyGenerator.CreateInterfaceProxyWithoutTarget(_serviceType,
                new DefaultRpcInterceptor(_serviceType, GetCurrentInvoker));

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
                _invoker = new LoadBalancedRpcInvoker(this, _loadBalancer, invokers).WithHandlers();
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