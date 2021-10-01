using Zoo.Rpc.Abstractions.Models;
using Zoo.Rpc.Abstractions.Nodes;

namespace Zoo.Rpc.Abstractions.LoadBalancers
{
    /// <summary>
    /// RPC load balancer.
    /// </summary>
    public interface IRpcLoadBalancer
    {
        /// <summary>
        /// Select.
        /// </summary>
        /// <param name="consumer"></param>
        /// <param name="invokers"></param>
        /// <param name="invocation"></param>
        /// <returns></returns>
        IRpcInvoker Select(IRpcConsumer consumer, IRpcInvoker[] invokers, IRpcInvocation invocation);
    }
}