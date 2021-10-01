using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;
using Zoo.Rpc.Abstractions.Attributes;
using Zoo.Rpc.Abstractions.Factories;
using Zoo.Rpc.Abstractions.Nodes;
using Zoo.Rpc.Core.Utils;

namespace Zoo.Rpc.Core.Factories
{
    /// <summary>
    /// Exporter factory.
    /// </summary>
    public class ExporterFactory : IRpcExporterFactory
    {
        private static readonly Type[] AllFactoryTypes = TypeUtils
            .ImplementationsOf<IRpcExporterFactory>()
            .ToArray();
        
        private static readonly ConcurrentDictionary<string, IRpcExporterFactory> Factories = new();

        private static IRpcExporterFactory Get(string scheme)
        {
            var factoryTypes = AllFactoryTypes
                .Where(type => type.GetCustomAttribute<SchemaAttribute>()?.Value == scheme)
                .ToArray();

            return factoryTypes.Length switch
            {
                0 => throw new InvalidOperationException($"No exporter is available for scheme: {scheme}"),
                > 1 => throw new InvalidOperationException($"Multiple exporter factories are available for scheme: {scheme}"),
                _ => (IRpcExporterFactory) Activator.CreateInstance(factoryTypes.First())
            };
        }

        public IRpcExporter Create(Type serviceType, Uri serviceUri, IRpcInvoker invoker)
        {
            return Factories.GetOrAdd(serviceUri.Scheme, Get).Create(serviceType, serviceUri, invoker);
        }
    }
}