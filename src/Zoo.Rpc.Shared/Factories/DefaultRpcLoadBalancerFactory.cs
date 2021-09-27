using System;
using System.Linq;
using System.Reflection;
using System.Web;
using Zoo.Rpc.Abstractions.Attributes;
using Zoo.Rpc.Abstractions.Constants;
using Zoo.Rpc.Abstractions.Factories;
using Zoo.Rpc.Abstractions.LoadBalancers;
using Zoo.Rpc.Shared.Utils;

namespace Zoo.Rpc.Shared.Factories
{
    /// <summary>
    /// Default load balancer factory.
    /// </summary>
    public class DefaultRpcLoadBalancerFactory : IRpcLoadBalancerFactory
    {
        public IRpcLoadBalancer Create(Type serviceType, Uri serviceUri)
        {
            var name =
                serviceType.GetCustomAttribute<LoadBalanceAttribute>()?.Name ??
                HttpUtility.ParseQueryString(serviceUri.Query).Get(ParameterNames.LoadBalance);

            var loadBalanceType = TypeUtils
                .ImplementationsOf<IRpcLoadBalancer>()
                .First(type => string.IsNullOrEmpty(name) || name == type.GetCustomAttribute<NameAttribute>()?.Value);

            return (IRpcLoadBalancer) Activator.CreateInstance(loadBalanceType);
        }
    }
}