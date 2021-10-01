using System;
using Zoo.Protocol.Http.Exporters;
using Zoo.Rpc.Abstractions.Attributes;
using Zoo.Rpc.Abstractions.Factories;
using Zoo.Rpc.Abstractions.Nodes;

namespace Zoo.Protocol.Http.Factories
{
    [Schema("https")]
    public class HttpsExporterFactory : IRpcExporterFactory
    {
        public IRpcExporter Create(Type serviceType, Uri serviceUri, IRpcInvoker invoker)
        {
            return new HttpExporter(serviceType, serviceUri, invoker);
        }
    }
}