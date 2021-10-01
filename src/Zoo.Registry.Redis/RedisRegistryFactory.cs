using System;
using Zoo.Rpc.Abstractions.Attributes;
using Zoo.Rpc.Abstractions.Factories;
using Zoo.Rpc.Abstractions.Nodes;

namespace Zoo.Registry.Redis
{
    /// <summary>
    /// Redis registry factory.
    /// </summary>
    [Schema("redis")]
    public class RedisRegistryFactory : IRpcRegistryFactory
    {
        /// <summary>
        /// Create a new registry instance.
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        public IRpcRegistry Create(Uri uri)
        {
            return new RedisRegistry(uri);
        }
    }
}