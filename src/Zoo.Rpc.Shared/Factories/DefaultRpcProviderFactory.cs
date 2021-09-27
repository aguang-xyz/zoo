using System;
using Zoo.Rpc.Abstractions.Factories;
using Zoo.Rpc.Abstractions.Nodes;
using Zoo.Rpc.Shared.Nodes;

namespace Zoo.Rpc.Shared.Factories
{
    /// <summary>
    /// Default provider factory.
    /// </summary>
    public class DefaultRpcProviderFactory : IRpcProviderFactory
    {
        public IRpcProvider Create(IRpcRegistry registry, Type serviceType, Uri serviceUri, object service)
        {
            return new DefaultRpcProvider(registry, serviceType, serviceUri, service);
        }
    }
}