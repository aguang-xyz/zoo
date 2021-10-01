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
    /// Registry factory.
    /// </summary>
    public class RegistryFactory : IRpcRegistryFactory
    {
        private static readonly Type[] AllFactoryTypes = TypeUtils
            .ImplementationsOf<IRpcRegistryFactory>()
            .ToArray();
        
        private static readonly ConcurrentDictionary<string, IRpcRegistryFactory> Factories = new();

        private static IRpcRegistryFactory GetFactory(string scheme)
        {
            var factoryTypes = AllFactoryTypes
                .Where(type => type.GetCustomAttribute<SchemaAttribute>()?.Value == scheme)
                .ToArray();

            return factoryTypes.Length switch
            {
                0 => throw new InvalidOperationException(
                    $"No registry factory is available for scheme: {scheme}"),
                > 1 => throw new InvalidOperationException(
                    $"Multiple registry factories are available for scheme: {scheme}"),
                _ => (IRpcRegistryFactory)Activator.CreateInstance(factoryTypes.First())
            };
        }

        public IRpcRegistry Create(Uri uri)
        {
            return Factories.GetOrAdd(uri.Scheme, GetFactory).Create(uri);
        }
    }
}