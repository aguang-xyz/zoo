using System;
using Zoo.Rpc.Abstractions.LoadBalancers;
using Zoo.Rpc.Abstractions.Models;
using Zoo.Rpc.Abstractions.Nodes;

namespace Zoo.Rpc.Shared.LoadBalancers
{
    /// <summary>
    /// Random load balancer.
    /// </summary>
    public class RandomLoadBalancer : IRpcLoadBalancer
    {
        private static readonly Random Random = new();
        
        public IRpcInvoker Select(Uri serviceUri, IRpcInvoker[] invokers, IRpcInvocation invocation)
        {
            if (invokers.Length == 0)
            {
                throw new InvalidOperationException("No invokers available");
            }
            
            return invokers[Random.Next(invokers.Length)];
        }
    }
}