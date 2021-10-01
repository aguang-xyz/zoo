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
    /// Invoker factory.
    /// </summary>
    public class InvokerFactory : IRpcInvokerFactory
    {
        private static readonly Type[] AllFactoryTypes = TypeUtils
            .ImplementationsOf<IRpcInvokerFactory>()
            .ToArray();
            
        private static readonly ConcurrentDictionary<string, IRpcInvokerFactory> Factories = new();

        private static IRpcInvokerFactory Get(string scheme)
        {
            var factoryTypes = AllFactoryTypes
                .Where(type => type.GetCustomAttribute<SchemaAttribute>()?.Value == scheme)
                .ToArray();

            return factoryTypes.Length switch
            {
                0 => throw new InvalidOperationException($"No invoker factory is available for scheme: {scheme}"),
                > 1 => throw new InvalidOperationException(
                    $"Multiple invoker factories are available for scheme: {scheme}"),
                _ => (IRpcInvokerFactory)Activator.CreateInstance(factoryTypes.First())
            };
        }

        public IRpcInvoker Create(Type serviceType, Uri serviceUri)
        {
            return Factories.GetOrAdd(serviceUri.Scheme, Get).Create(serviceType, serviceUri);
        }
    }
}