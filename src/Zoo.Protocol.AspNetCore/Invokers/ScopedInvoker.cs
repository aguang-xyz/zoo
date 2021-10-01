using System;
using Microsoft.Extensions.DependencyInjection;
using Zoo.Rpc.Abstractions.Models;
using Zoo.Rpc.Abstractions.Nodes;
using Zoo.Rpc.Core.Invokers;

namespace Zoo.Protocol.AspNetCore.Invokers
{
    public class ScopedInvoker : IRpcInvoker
    {
        private readonly Type _serviceType;
        
        private readonly IServiceProvider _serviceProvider;
        
        public ScopedInvoker(Uri uri, Type serviceType, IServiceProvider serviceProvider)
        {
            Uri = uri;
            _serviceType = serviceType;
            _serviceProvider = serviceProvider;
        }

        public Uri Uri { get; }

        public bool IsConsumerSide => true;
        
        public IRpcResult Invoke(IRpcInvocation invocation)
        {
            // Create a new scope.
            using var scope = _serviceProvider.CreateScope();

            // Invoke.
            var service = scope.ServiceProvider.GetRequiredService(_serviceType);
            var objectInvoker = new ObjectInvoker(Uri, IsConsumerSide, service);
            
            return objectInvoker.Invoke(invocation);
        }
        
        public void Dispose()
        {
        }
    }
}