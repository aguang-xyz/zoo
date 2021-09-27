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
    /// Default registry factory.
    /// </summary>
    public class DefaultRpcRegistryFactory : IRpcRegistryFactory
    {
        private static readonly ConcurrentDictionary<string, IRpcRegistryFactory> Factories = new();

        private static IRpcRegistryFactory Get(string scheme)
        {
            var factoryTypes = TypeUtils
                .ImplementationsOf<IRpcRegistryFactory>()
                .Where(type => type.GetCustomAttribute<SchemaAttribute>()?.Value == scheme)
                .ToArray();

            return factoryTypes.Length switch
            {
                0 => throw new InvalidOperationException($"No rpc registry factory is available for scheme: {scheme}"),
                > 1 => throw new InvalidOperationException(
                    $"Multiple rpc registry factories are available for scheme: {scheme}"),
                _ => (IRpcRegistryFactory)Activator.CreateInstance(factoryTypes.First())
            };
        }

        private static IRpcRegistryFactory GetWithCache(string scheme)
        {
            return Factories.GetOrAdd(scheme, Get);
        }
        
        public IRpcRegistry Create(Uri uri)
        {
            return GetWithCache(uri.Scheme).Create(uri);
        }
    }
}