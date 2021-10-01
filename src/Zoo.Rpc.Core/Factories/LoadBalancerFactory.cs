using System;
using System.Linq;
using System.Reflection;
using System.Web;
using Zoo.Rpc.Abstractions.Attributes;
using Zoo.Rpc.Abstractions.Constants;
using Zoo.Rpc.Abstractions.Factories;
using Zoo.Rpc.Abstractions.LoadBalancers;
using Zoo.Rpc.Core.Utils;

namespace Zoo.Rpc.Core.Factories
{
    /// <summary>
    /// Load balancer factory.
    /// </summary>
    public class LoadBalancerFactory : IRpcLoadBalancerFactory
    {
        private static readonly Type[] AllTypes = TypeUtils
            .ImplementationsOf<IRpcLoadBalancer>()
            .ToArray();
        
        public IRpcLoadBalancer Create(Type serviceType, Uri serviceUri)
        {
            var name =
                serviceType.GetCustomAttribute<LoadBalanceAttribute>()?.Name ??
                HttpUtility.ParseQueryString(serviceUri.Query).Get(ParameterNames.LoadBalance);

            // When load balancer is not specified, any load balancer implementation is acceptable.
            if (string.IsNullOrEmpty(name))
            {
                if (AllTypes.Length == 0)
                {
                    throw new InvalidOperationException("No load balancer factory is found");
                }

                return (IRpcLoadBalancer) Activator.CreateInstance(AllTypes[0]);
            }
            
            var types = AllTypes
                .Where(type => type.GetCustomAttribute<NameAttribute>()?.Value == name)
                .ToArray();

            return types.Length switch
            {
                0 => throw new InvalidOperationException($"No load balancer factory is found for name: {name}"),
                > 1 => throw new InvalidOperationException($"Multiple balancer factories are found for name: {name}"),
                _ => (IRpcLoadBalancer)Activator.CreateInstance(types[0])
            };
        }
    }
}