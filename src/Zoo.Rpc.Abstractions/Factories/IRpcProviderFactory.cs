using System;
using Zoo.Rpc.Abstractions.Nodes;

namespace Zoo.Rpc.Abstractions.Factories
{
    /// <summary>
    /// Provider factory.
    /// </summary>
    public interface IRpcProviderFactory
    {
        /// <summary>
        /// Create.
        /// </summary>
        /// <param name="registry"></param>
        /// <param name="serviceType"></param>
        /// <param name="serviceUri"></param>
        /// <param name="service"></param>
        /// <returns></returns>
        IRpcProvider Create(IRpcRegistry registry, Type serviceType, Uri serviceUri, object service);
    }
}