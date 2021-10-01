using System;
using Zoo.Rpc.Abstractions.Attributes;
using Zoo.Rpc.Abstractions.Factories;
using Zoo.Rpc.Abstractions.Nodes;

namespace Zoo.Registry.InMemory
{
    [Schema("in-memory")]
    public class InMemoryRegistryFactory : IRpcRegistryFactory
    {
        public IRpcRegistry Create(Uri uri)
        {
            return new InMemoryRegistry(uri);
        }
    }
}