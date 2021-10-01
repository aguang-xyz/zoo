using System;
using Zoo.Rpc.Abstractions.Nodes;

namespace Zoo.Rpc.Abstractions.Factories
{
    /// <summary>
    /// RPC invoker factory.
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