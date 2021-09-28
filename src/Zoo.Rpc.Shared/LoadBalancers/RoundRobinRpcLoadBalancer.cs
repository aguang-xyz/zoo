using Zoo.Rpc.Abstractions.Attributes;
using Zoo.Rpc.Abstractions.Models;
using Zoo.Rpc.Abstractions.Nodes;

namespace Zoo.Rpc.Shared.LoadBalancers
{
    /// <summary>
    /// Round-robin load balancer.
    /// </summary>
    [Name("round-robin")]
    public class RoundRobinRpcLoadBalancer : RpcLoadBalancerBase
    {
        private int _index;
        
        protected override IRpcInvoker DoSelect(IRpcConsumer consumer, IRpcInvoker[] invokers, IRpcInvocation invocation)
        {
            lock (this)
            {
                return invokers[_index++ % invokers.Length];
            }
        }
    }
}