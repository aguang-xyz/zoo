using System;
using Zoo.Rpc.Abstractions.LoadBalancers;
using Zoo.Rpc.Abstractions.Models;
using Zoo.Rpc.Abstractions.Nodes;

namespace Zoo.Rpc.Shared.Nodes
{
    /// <summary>
    /// Invokers behind a load balancer.
    /// </summary>
    public class LoadBalancedRpcInvoker : IRpcInvoker
    {
        private readonly IRpcConsumer _consumer;
        
        private readonly IRpcLoadBalancer _loadBalancer;

        private readonly IRpcInvoker[] _innerInvokers;

        public LoadBalancedRpcInvoker(IRpcConsumer consumer, IRpcLoadBalancer loadBalancer, IRpcInvoker[] innerInvokers)
        {
            _consumer = consumer;
            _loadBalancer = loadBalancer;
            _innerInvokers = innerInvokers;
            Uri = _consumer.Uri;
        }

        public bool IsConsumerSide => true;

        public Uri Uri { get; }

        public IRpcResult Invoke(IRpcInvocation invocation)
        {
            return _loadBalancer.Select(_consumer, _innerInvokers, invocation).Invoke(invocation);
        }
        
        public void Dispose()
        {
        }
    }
}