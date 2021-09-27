using System;
using Castle.DynamicProxy;
using Microsoft.Extensions.DependencyInjection;

namespace Zoo.Protocol.AspNetCore.Interceptors
{
    public class AspNetCoreRpcInterceptor : IInterceptor
    {
        private readonly IServiceProvider _serviceProvider;

        private readonly Type _serviceType;

        public AspNetCoreRpcInterceptor(IServiceProvider serviceProvider, Type serviceType)
        {
            _serviceProvider = serviceProvider;
            _serviceType = serviceType;
        }

        public void Intercept(IInvocation invocation)
        {
            // Create a new scope.
            using var scope = _serviceProvider.CreateScope();

            // Retrieve service and invoke.
            invocation.ReturnValue = invocation.Method
                .Invoke(scope.ServiceProvider.GetRequiredService(_serviceType), invocation.Arguments);
        }
    }
}