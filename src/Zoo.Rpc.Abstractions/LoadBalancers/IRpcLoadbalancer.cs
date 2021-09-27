using System;
using Zoo.Rpc.Abstractions.Models;
using Zoo.Rpc.Abstractions.Nodes;

namespace Zoo.Rpc.Abstractions.LoadBalancers
{
    /// <summary>
    /// Load balancer.
    /// </summary>
    public interface IRpcLoadBalancer
    {
        /// <summary>
        /// Select.
        /// </summary>
        /// <param name="serviceUri"></param>
        /// <param name="invokers"></param>
        /// <param name="invocation"></param>
        /// <returns></returns>
        IRpcInvoker Select(Uri serviceUri, IRpcInvoker[] invokers, IRpcInvocation invocation);
    }
}