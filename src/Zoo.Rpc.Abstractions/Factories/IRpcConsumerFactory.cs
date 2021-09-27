using System;
using Zoo.Rpc.Abstractions.Nodes;

namespace Zoo.Rpc.Abstractions.Factories
{
    /// <summary>
    /// Consumer factory.
    /// </summary>
    public interface IRpcConsumerFactory
    {
        /// <summary>
        /// Create.
        /// </summary>
        /// <param name="registry"></param>
        /// <param name="serviceType"></param>
        /// <param name="serviceUri"></param>
        /// <returns></returns>
        IRpcConsumer Create(IRpcRegistry registry, Type serviceType, Uri serviceUri);
    }
}