using System;
using Zoo.Protocol.AspNetCore.Exporters;
using Zoo.Rpc.Abstractions.Attributes;
using Zoo.Rpc.Abstractions.Factories;
using Zoo.Rpc.Abstractions.Nodes;

namespace Zoo.Protocol.AspNetCore.Factories
{
    [Schema("http")]
    public class HttpExporterFactory : IRpcExporterFactory
    {
        public IRpcExporter Create(Type serviceType, Uri serviceUri, IRpcInvoker invoker)
        {
            return new HttpExporter(serviceType, serviceUri, invoker);
        }
    }
}