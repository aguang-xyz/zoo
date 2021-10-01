using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Zoo.Rpc.Abstractions.Models;
using Zoo.Rpc.Abstractions.Nodes;
using Zoo.Rpc.Core.Models;

namespace Zoo.Rpc.Core.Invokers
{
    /// <summary>
    /// Object invoker. 
    /// </summary>
    public class ObjectInvoker : IRpcInvoker
    {
        public ObjectInvoker(Uri uri, bool isConsumerSide, object target)
        {
            Uri = uri;
            IsConsumerSide = isConsumerSide;
            Target = target;
        }

        public Uri Uri { get; }

        public bool IsConsumerSide { get; }
        
        public object Target { get; }

        private static bool IsMatchedMethod(IEnumerable<Type> parameterTypes, MethodBase methodInfo)
        {
            return methodInfo
                .GetParameters()
                .Select(parameterInfo => parameterInfo.ParameterType)
                .SequenceEqual(parameterTypes);
        }

        private static MethodInfo GetMethod(IRpcInvocation rpcInvocation)
        {
            return rpcInvocation
                .ServiceType
                .GetMethods()
                .Where(methodInfo => methodInfo.Name == rpcInvocation.MethodName)
                .First(methodInfo => IsMatchedMethod(rpcInvocation.ParameterTypes, methodInfo));
        }
        
        public IRpcResult Invoke(IRpcInvocation invocation)
        {
            try
            {
                return new RpcResult
                {
                    ReturnValue = GetMethod(invocation).Invoke(Target, invocation.Parameters),
                    Attachments = new Dictionary<string, string>()
                };
            }
            catch (TargetInvocationException e)
            {
                return new RpcResult
                {
                    ReturnValue = null,
                    Exception = e.InnerException,
                    Attachments = new Dictionary<string, string>()
                };
            }
            catch (Exception e)
            {
                return new RpcResult
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