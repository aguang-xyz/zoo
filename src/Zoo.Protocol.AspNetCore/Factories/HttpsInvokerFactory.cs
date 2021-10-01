using System;
using Zoo.Protocol.AspNetCore.Invokers;
using Zoo.Rpc.Abstractions.Attributes;
using Zoo.Rpc.Abstractions.Factories;
using Zoo.Rpc.Abstractions.Nodes;

namespace Zoo.Protocol.AspNetCore.Factories
{
    [Schema("https")]
    public class HttpsInvokerFactory : IRpcInvokerFactory
    {
        public IRpcInvoker Create(Type serviceType, Uri serviceUri)
        {
            return new HttpInvoker(serviceType, serviceUri);
        }
    }
}