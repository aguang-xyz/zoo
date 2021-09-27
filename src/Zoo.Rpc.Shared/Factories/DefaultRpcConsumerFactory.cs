using System;
using Zoo.Rpc.Abstractions.Factories;
using Zoo.Rpc.Abstractions.Nodes;
using Zoo.Rpc.Shared.Nodes;

namespace Zoo.Rpc.Shared.Factories
{
    /// <summary>
    /// Default consumer factory.
    /// </summary>
    public class DefaultRpcConsumerFactory : IRpcConsumerFactory
    {
        public IRpcConsumer Create(IRpcRegistry registry, Type serviceType, Uri serviceUri)
        {
            return new DefaultRpcConsumer(registry, serviceType, serviceUri);
        }
    }
}