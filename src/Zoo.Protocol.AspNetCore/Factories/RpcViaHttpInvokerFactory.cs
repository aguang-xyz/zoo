using System;
using Zoo.Protocol.AspNetCore.Nodes;
using Zoo.Rpc.Abstractions.Attributes;
using Zoo.Rpc.Abstractions.Factories;
using Zoo.Rpc.Abstractions.Nodes;

namespace Zoo.Protocol.AspNetCore.Factories
{
    [Schema("http")]
    public class RpcViaHttpInvokerFactory : IRpcInvokerFactory
    {
        public IRpcInvoker Create(Type serviceType, Uri serviceUri)
        {
            return new RpcViaHttpInvoker(serviceType, serviceUri);
        }
    }
}