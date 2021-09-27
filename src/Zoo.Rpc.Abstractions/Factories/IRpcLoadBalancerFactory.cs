using System;
using Zoo.Rpc.Abstractions.LoadBalancers;

namespace Zoo.Rpc.Abstractions.Factories
{
    /// <summary>
    /// Load balancer factory.
    /// </summary>
    public interface IRpcLoadBalancerFactory
    {
        /// <summary>
        /// Create.
        /// </summary>
        /// <param name="serviceType"></param>
        /// <param name="serviceUri"></param>
        /// <returns></returns>
        IRpcLoadBalancer Create(Type serviceType, Uri serviceUri);
    }
}