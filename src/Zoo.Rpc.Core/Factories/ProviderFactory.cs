using System;
using Zoo.Rpc.Abstractions.Factories;
using Zoo.Rpc.Abstractions.Nodes;
using Zoo.Rpc.Core.Nodes;

namespace Zoo.Rpc.Core.Factories
{
    /// <summary>
    /// Provider factory.
    /// </summary>
    public class ProviderFactory : IRpcProviderFactory
    {
        public IRpcProvider Create(IRpcRegistry registry, Type serviceType, Uri serviceUri, IRpcInvoker invoker)
        {
            return new Provider(registry, serviceType, serviceUri, invoker);
        }
    }
}