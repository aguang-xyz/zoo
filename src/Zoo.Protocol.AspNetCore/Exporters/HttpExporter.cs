using System;
using Zoo.Protocol.AspNetCore.Middlewares;
using Zoo.Rpc.Abstractions.Nodes;

namespace Zoo.Protocol.AspNetCore.Exporters
{
    public class HttpExporter : IRpcExporter
    {
        private readonly Type _serviceType;

        private readonly IRpcInvoker _invoker;
        
        public HttpExporter(Type serviceType, Uri serviceUri, IRpcInvoker invoker)
        {
            _serviceType = serviceType;
            _invoker = invoker;
            Uri = serviceUri;
        }

        public Uri Uri { get; }
        
        public void Start()
        {
            RpcMiddleware.RegisterInvoker(_serviceType, _invoker);
        }
        
        public void Dispose()
        {
            RpcMiddleware.UnregisterService(_serviceType);
        }
    }
}