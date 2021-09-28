using System;
using Zoo.Rpc.Abstractions.Attributes;
using Zoo.Rpc.Abstractions.Models;
using Zoo.Rpc.Abstractions.Nodes;

namespace Zoo.Rpc.Shared.LoadBalancers
{
    /// <summary>
    /// Random load balancer.
    /// </summary>
    [Name("random")]
    public class RandomLoadRpcBalancer : RpcLoadBalancerBase
    {
        private static readonly Random Random = new();
        
        protected override IRpcInvoker DoSelect(IRpcConsumer consumer, IRpcInvoker[] invokers, IRpcInvocation invocation)
        {
            return invokers[Random.Next(invokers.Length)];
        }
    }
}