using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;
using Zoo.Rpc.Abstractions.Attributes;
using Zoo.Rpc.Abstractions.Factories;
using Zoo.Rpc.Abstractions.Nodes;
using Zoo.Rpc.Shared.Utils;

namespace Zoo.Rpc.Shared.Factories
{
    /// <summary>
    /// Default exporter factory.
    /// </summary>
    public class DefaultRpcExporterFactory : IRpcExporterFactory
    {
        private static readonly ConcurrentDictionary<string, IRpcExporterFactory> Factories = new();

        private static IRpcExporterFactory Get(string scheme)
        {
            var factoryTypes = TypeUtils
                .ImplementationsOf<IRpcExporterFactory>()
                .Where(type => type.GetCustomAttribute<SchemaAttribute>()?.Value == scheme)
                .ToArray();

            return factoryTypes.Length switch
            {
                0 => throw new InvalidOperationException($"No rpc exporter is available for scheme: {scheme}"),
                > 1 => throw new InvalidOperationException($"Multiple rpc exporter factories are available for scheme: {scheme}"),
                _ => (IRpcExporterFactory) Activator.CreateInstance(factoryTypes.First())
            };
        }

        private static IRpcExporterFactory GetWithCache(string scheme)
        {
            return Factories.GetOrAdd(scheme, Get);
        }

        public IRpcExporter Create(Type serviceType, Uri serviceUri, IRpcInvoker invoker)
        {
            return GetWithCache(serviceUri.Scheme).Create(serviceType, serviceUri, invoker);
        }
    }
}