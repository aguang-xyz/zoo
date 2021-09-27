using System;
using Zoo.Rpc.Abstractions.Factories;
using Zoo.Rpc.Abstractions.Nodes;
using Zoo.Rpc.Shared.Extensions;
using Zoo.Rpc.Shared.Factories;
using Zoo.Rpc.Shared.Utils;

namespace Zoo.Rpc.Shared.Nodes
{
    /// <summary>
    /// Default provider.
    /// </summary>
    public class DefaultRpcProvider : IRpcProvider
    {
        private static readonly IRpcExporterFactory ExporterFactory = new DefaultRpcExporterFactory();

        private readonly IRpcRegistry _registry;

        private readonly IRpcExporter _exporter;

        public DefaultRpcProvider(IRpcRegistry registry, Type serviceType, Uri serviceUri, object service)
        {
            _registry = registry;
            _exporter = ExporterFactory.Create(serviceType, serviceUri, new ClrRpcInvoker(service).WithHandlers());
            Uri = UriUtils.CreateProviderUri(serviceType, serviceUri);
        }
        
        public Uri Uri { get; }

        public void Start()
        {
            // Export the service.
            _exporter.Start();
            
            // Register the provider node.
            _registry.Register(Uri);
        }
        
        public void Dispose()
        {
            // Unregister the provider node.
            _registry.Unregister(Uri);
            
            // Stop exporting the service.
            _exporter.Dispose();
        }
    }
}