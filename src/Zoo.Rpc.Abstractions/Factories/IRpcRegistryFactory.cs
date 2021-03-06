using System;
using Zoo.Rpc.Abstractions.Nodes;

namespace Zoo.Rpc.Abstractions.Factories
{
    /// <summary>
    /// RPC registry factory.
    /// </summary>
    public interface IRpcRegistryFactory
    {
        /// <summary>
        /// Create.
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        IRpcRegistry Create(Uri uri);
    }
}