using System;
using Zoo.Rpc.Abstractions.Factories;
using Zoo.Rpc.Abstractions.Nodes;
using Zoo.Rpc.Core.Factories;
using Zoo.Rpc.Core.Utils;

namespace Zoo.Rpc.Core.Nodes
{
    /// <summary>
    /// Provider.
    /// </summary>
    public class Provider : IRpcProvider
    {
        private static readonly IRpcExporterFactory ExporterFactory = new ExporterFactory();

        private readonly IRpcRegistry _registry;

        private readonly IRpcExporter _exporter;

        public Provider(IRpcRegistry registry, Type serviceType, Uri serviceUri, IRpcInvoker invoker)
        {
            _registry = registry;
            _exporter = ExporterFactory.Create(serviceType, serviceUri, invoker);
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