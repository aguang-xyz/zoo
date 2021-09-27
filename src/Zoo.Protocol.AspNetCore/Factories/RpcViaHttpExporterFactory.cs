using System;
using Zoo.Protocol.AspNetCore.Nodes;
using Zoo.Rpc.Abstractions.Attributes;
using Zoo.Rpc.Abstractions.Factories;
using Zoo.Rpc.Abstractions.Nodes;

namespace Zoo.Protocol.AspNetCore.Factories
{
    [Schema("http")]
    public class RpcViaHttpExporterFactory : IRpcExporterFactory
    {
        public IRpcExporter Create(Type serviceType, Uri serviceUri, IRpcInvoker invoker)
        {
            return new RpcViaHttpExporter(serviceType, serviceUri, invoker);
        }
    }
}