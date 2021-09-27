using System;
using System.Collections.Generic;
using System.Linq;
using Castle.DynamicProxy;
using Zoo.Rpc.Abstractions.Nodes;
using Zoo.Rpc.Shared.Models;

namespace Zoo.Rpc.Shared.Interceptors
{
    /// <summary>
    /// Interceptor (for dynamic proxy).
    /// </summary>
    public class DefaultRpcInterceptor : IInterceptor
    {
        private readonly Type _serviceType;
        
        private readonly Func<IRpcInvoker> _getInvoker;

        public DefaultRpcInterceptor(Type serviceType, Func<IRpcInvoker> getInvoker)
        {
            _serviceType = serviceType;
            _getInvoker = getInvoker;
        }

        public void Intercept(IInvocation invocation)
        {
            var rpcInvocation = new DefaultRpcInvocation
            {
                ServiceTypeName = _serviceType.FullName!,
                MethodName = invocation.Method.Name,
                ParameterTypeNames = invocation.Method.GetParameters().Select(parameterInfo => parameterInfo.ParameterType.FullName).ToArray(),
                Parameters = invocation.Arguments,
                Attachments = new Dictionary<string, string>()
            };

            var rpcResult = _getInvoker().Invoke(rpcInvocation);

            if (rpcResult.Exception == default)
            {
                invocation.ReturnValue = rpcResult.ReturnValue;
            }
            else
            {
                throw rpcResult.Exception;
            }
        }
    }
}