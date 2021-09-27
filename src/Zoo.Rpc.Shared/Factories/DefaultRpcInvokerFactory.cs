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
    /// Default invoker factory.
    /// </summary>
    public class DefaultRpcInvokerFactory : IRpcInvokerFactory
    {
        private static readonly ConcurrentDictionary<string, IRpcInvokerFactory> Factories = new();

        private static IRpcInvokerFactory Get(string scheme)
        {
            var factoryTypes = TypeUtils
                .ImplementationsOf<IRpcInvokerFactory>()
                .Where(type => type.GetCustomAttribute<SchemaAttribute>()?.Value == scheme)
                .ToArray();

            return factoryTypes.Length switch
            {
                0 => throw new InvalidOperationException($"No rpc invoker factory is available for scheme: {scheme}"),
                > 1 => throw new InvalidOperationException(
                    $"Multiple rpc invoker factories are available for scheme: {scheme}"),
                _ => (IRpcInvokerFactory)Activator.CreateInstance(factoryTypes.First())
            };
        }

        private static IRpcInvokerFactory GetWithCache(string scheme)
        {
            return Factories.GetOrAdd(scheme, Get);
        }

        public IRpcInvoker Create(Type serviceType, Uri serviceUri)
        {
            return GetWithCache(serviceUri.Scheme).Create(serviceType, serviceUri);
        }
    }
}