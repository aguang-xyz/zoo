using System;
using System.Linq;
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
        private readonly IRpcLoadBalancer _loadBalancer;

        private readonly IRpcInvoker[] _innerInvokers;

        public LoadBalancedRpcInvoker(IRpcLoadBalancer loadBalancer, IRpcInvoker[] innerInvokers)
        {
            _loadBalancer = loadBalancer;
            _innerInvokers = innerInvokers;
        }

        public bool IsConsumerSide => true;

        public Uri Uri
        {
            get
            {
                if (_innerInvokers.Any())
                {
                    return _innerInvokers.First().Uri;
                }

                throw new InvalidOperationException("No inner invokers available");
            }
        }

        public IRpcResult Invoke(IRpcInvocation invocation)
        {
            return _loadBalancer.Select(Uri, _innerInvokers, invocation).Invoke(invocation);
        }
        
        public void Dispose()
        {
        }
    }
}