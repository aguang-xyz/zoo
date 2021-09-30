using System;
using Microsoft.Extensions.DependencyInjection;
using Zoo.Rpc.Abstractions.Models;
using Zoo.Rpc.Abstractions.Nodes;
using Zoo.Rpc.Shared.Nodes;

namespace Zoo.Protocol.AspNetCore.Nodes
{
    public class AspNetCoreInvoker : IRpcInvoker
    {
        private readonly Type _serviceType;
        
        private readonly IServiceProvider _serviceProvider;
        
        public AspNetCoreInvoker(Type serviceType, IServiceProvider serviceProvider)
        {
            _serviceType = serviceType;
            _serviceProvider = serviceProvider;
        }

        public Uri Uri => throw new NotImplementedException();

        public bool IsConsumerSide => true;
        
        public IRpcResult Invoke(IRpcInvocation invocation)
        {
            // Create a new scope.
            using var scope = _serviceProvider.CreateScope();

            // Invoke.
            return new ClrRpcInvoker(scope.ServiceProvider.GetService(_serviceType)).Invoke(invocation);
        }
        
        public void Dispose()
        {
        }
    }
}