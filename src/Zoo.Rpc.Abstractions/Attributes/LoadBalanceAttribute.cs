using System;

namespace Zoo.Rpc.Abstractions.Attributes
{
    /// <summary>
    /// Load balance attribute, this will be used on top of service interface.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class LoadBalanceAttribute : Attribute
    {
        /// <summary>
        /// Load balance attribute.
        /// </summary>
        /// <param name="name">the name of load balancer that you want to perform for a specify rpc service</param>
        public LoadBalanceAttribute(string name)
        {
            Name = name;
        }

        public string Name { get; }
    }
}