using System;
using Zoo.Rpc.Abstractions.Nodes;

namespace Zoo.Rpc.Abstractions.Factories
{
    /// <summary>
    /// Invoker factory.
    /// </summary>
    public interface IRpcInvokerFactory
    {
        /// <summary>
        /// Create.
        /// </summary>
        /// <param name="serviceType"></param>
        /// <param name="serviceUri"></param>
        /// <returns></returns>
        IRpcInvoker Create(Type serviceType, Uri serviceUri);
    }
}