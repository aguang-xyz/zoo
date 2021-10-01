using Zoo.Rpc.Abstractions.Attributes;
using Zoo.Rpc.Abstractions.Models;
using Zoo.Rpc.Abstractions.Nodes;

namespace Zoo.Rpc.Core.LoadBalancers
{
    /// <summary>
    /// Round-robin load balancer.
    /// </summary>
    [Name("round-robin")]
    public class RoundRobinRpcLoadBalancer : FilteredRpcLoadBalancer
    {
        private int _index;
        
        protected override IRpcInvoker DoSelect(IRpcConsumer rpcConsumer, IRpcInvoker[] invokers, IRpcInvocation rpcInvocation)
        {
            lock (this)
            {
                return invokers[_index++ % invokers.Length];
            }
        }
    }
}