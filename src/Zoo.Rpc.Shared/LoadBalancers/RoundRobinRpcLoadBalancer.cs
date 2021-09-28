using System;
using Zoo.Rpc.Abstractions.Attributes;
using Zoo.Rpc.Abstractions.LoadBalancers;
using Zoo.Rpc.Abstractions.Models;
using Zoo.Rpc.Abstractions.Nodes;

namespace Zoo.Rpc.Shared.LoadBalancers
{
    /// <summary>
    /// Round-robin load balancer.
    /// </summary>
    [Name("round-robin")]
    public class RoundRobinRpcLoadBalancer : IRpcLoadBalancer
    {
        private int _index;
        
        public IRpcInvoker Select(Uri serviceUri, IRpcInvoker[] invokers, IRpcInvocation invocation)
        {
            if (invokers.Length == 0)
            {
                throw new InvalidOperationException("No invokers available");
            }
            
            lock (this)
            {
                return invokers[_index++ % invokers.Length];
            }
        }
    }
}