using System;
using Zoo.Rpc.Abstractions.Factories;
using Zoo.Rpc.Abstractions.Nodes;
using Zoo.Rpc.Core.Nodes;

namespace Zoo.Rpc.Core.Factories
{
    /// <summary>
    /// Consumer factory.
    /// </summary>
    public class ConsumerFactory : IRpcConsumerFactory
    {
        public IRpcConsumer Create(IRpcRegistry registry, Type serviceType, Uri serviceUri)
        {
            return new Consumer(registry, serviceType, serviceUri);
        }
    }
}