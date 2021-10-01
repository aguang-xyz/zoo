using System;
using Zoo.Rpc.Abstractions.LoadBalancers;
using Zoo.Rpc.Abstractions.Models;
using Zoo.Rpc.Abstractions.Nodes;

namespace Zoo.Rpc.Core.Invokers
{
    /// <summary>
    /// Invokers behind a load balancer.
    /// </summary>
    public class LoadBalancedInvoker : IRpcInvoker
    {
        private readonly IRpcConsumer _rpcConsumer;
        
        private readonly IRpcLoadBalancer _rpcLoadBalancer;

        private readonly IRpcInvoker[] _innerInvokers;

        public LoadBalancedInvoker(IRpcConsumer rpcConsumer, IRpcLoadBalancer rpcLoadBalancer, IRpcInvoker[] innerInvokers)
        {
            _rpcConsumer = rpcConsumer;
            _rpcLoadBalancer = rpcLoadBalancer;
            _innerInvokers = innerInvokers;
            Uri = _rpcConsumer.Uri;
        }

        public bool IsConsumerSide => true;

        public Uri Uri { get; }

        public IRpcResult Invoke(IRpcInvocation invocation)
        {
            return _rpcLoadBalancer.Select(_rpcConsumer, _innerInvokers, invocation).Invoke(invocation);
        }
        
        public void Dispose()
        {
        }
    }
}