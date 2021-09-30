using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Zoo.Rpc.Abstractions.Models;
using Zoo.Rpc.Abstractions.Nodes;
using Zoo.Rpc.Shared.Models;

namespace Zoo.Rpc.Shared.Nodes
{
    /// <summary>
    /// In runtime invoker (provider side invoker). 
    /// </summary>
    public class ClrRpcInvoker : IRpcInvoker
    {
        private readonly object _service;

        public ClrRpcInvoker(object service)
        {
            _service = service;
        }

        public Uri Uri => throw new NotImplementedException();

        public bool IsConsumerSide => false;
        
        private static bool IsMatchedMethod(IReadOnlyList<Type> parameterTypes, MethodBase methodInfo)
        {
            var parameters = methodInfo.GetParameters();

            if (parameters.Length != parameterTypes.Count)
            {
                return false;
            }

            for (var i = 0; i < parameters.Length; i++)
            {
                if (parameters[i].ParameterType != parameterTypes[i])
                {
                    return false;
                }
            }

            return true;
        }

        private static MethodInfo GetMethod(IRpcInvocation invocation)
        {
            return invocation
                .ServiceType
                .GetMethods()
                .Where(methodInfo => methodInfo.Name == invocation.MethodName)
                .First(methodInfo => IsMatchedMethod(invocation.ParameterTypes, methodInfo));
        }
        
        public IRpcResult Invoke(IRpcInvocation invocation)
        {
            try
            {
                return new DefaultRpcResult
                {
                    ReturnValue = GetMethod(invocation).Invoke(_service, invocation.Parameters),
                    Attachments = new Dictionary<string, string>()
                };
            }
            catch (TargetInvocationException e)
            {
                return new DefaultRpcResult
                {
                    ReturnValue = null,
                    Exception = e.InnerException,
                    Attachments = new Dictionary<string, string>()
                };
            }
            catch (Exception e)
            {
                return new DefaultRpcResult
                {
                    ReturnValue = null,
                    Exception = e,
                    Attachments = new Dictionary<string, string>()
                };
            }
        }

        public void Dispose()
        {
        }
    }
}