using System;
using Zoo.Protocol.Http.Invokers;
using Zoo.Rpc.Abstractions.Attributes;
using Zoo.Rpc.Abstractions.Factories;
using Zoo.Rpc.Abstractions.Nodes;

namespace Zoo.Protocol.Http.Factories
{
    [Schema("http")]
    public class HttpInvokerFactory : IRpcInvokerFactory
    {
        public IRpcInvoker Create(Type serviceType, Uri serviceUri)
        {
            return new HttpInvoker(serviceType, serviceUri);
        }
    }
}