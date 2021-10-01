using System;
using Zoo.Rpc.Abstractions.Attributes;
using Zoo.Rpc.Abstractions.Models;
using Zoo.Rpc.Abstractions.Nodes;

namespace Zoo.Rpc.Core.LoadBalancers
{
    /// <summary>
    /// Random load balancer.
    /// </summary>
    [Name("random")]
    public class RandomRpcLoadBalancer : FilteredRpcLoadBalancer
    {
        private static readonly Random Random = new();
        
        protected override IRpcInvoker DoSelect(IRpcConsumer rpcConsumer, IRpcInvoker[] invokers, IRpcInvocation rpcInvocation)
        {
            return invokers[Random.Next(invokers.Length)];
        }
    }
}