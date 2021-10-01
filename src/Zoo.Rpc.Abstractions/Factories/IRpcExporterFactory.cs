using System;
using Zoo.Rpc.Abstractions.Nodes;

namespace Zoo.Rpc.Abstractions.Factories
{
    /// <summary>
    /// RPC exporter factory.
    /// </summary>
    public interface IRpcExporterFactory
    {
        /// <summary>
        /// Create.
        /// </summary>
        /// <param name="invoker"></param>
        /// <param name="serviceType"></param>
        /// <param name="serviceUri"></param>
        /// <returns></returns>
        IRpcExporter Create(Type serviceType, Uri serviceUri, IRpcInvoker invoker);
    }
}