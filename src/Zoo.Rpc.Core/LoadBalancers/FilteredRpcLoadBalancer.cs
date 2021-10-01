using System;
using System.Linq;
using Zoo.Rpc.Abstractions.Extensions;
using Zoo.Rpc.Abstractions.LoadBalancers;
using Zoo.Rpc.Abstractions.Models;
using Zoo.Rpc.Abstractions.Nodes;

namespace Zoo.Rpc.Core.LoadBalancers
{
    public abstract class FilteredRpcLoadBalancer : IRpcLoadBalancer
    {
        public IRpcInvoker Select(IRpcConsumer consumer, IRpcInvoker[] invokers, IRpcInvocation invocation)
        {
            // Filter out service providers where the service version is mis-matched.
             invokers = invokers
                .Where(invoker => invoker.Uri.GetServiceVersion() == consumer.Uri.GetServiceVersion())
                .ToArray();
             
             if (invokers.Length == 0)
             {
                 throw new InvalidOperationException("No invokers available");
             }

             return DoSelect(consumer, invokers, invocation);
        }

        protected abstract IRpcInvoker DoSelect(IRpcConsumer rpcConsumer, IRpcInvoker[] invokers,
            IRpcInvocation rpcInvocation);
    }
}