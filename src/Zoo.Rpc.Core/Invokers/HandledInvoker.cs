using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Zoo.Rpc.Abstractions.Attributes;
using Zoo.Rpc.Abstractions.Handlers;
using Zoo.Rpc.Abstractions.Models;
using Zoo.Rpc.Abstractions.Nodes;
using Zoo.Rpc.Core.Models;
using Zoo.Rpc.Core.Utils;

namespace Zoo.Rpc.Core.Invokers
{
    /// <summary>
    /// Invoker behind a handler.
    /// </summary>
    public class HandledInvoker : IRpcInvoker
    {
        private static readonly Type[] HandlerTypes = TypeUtils
            .ImplementationsOf<IRpcHandler>()
            .OrderBy(type => type.GetCustomAttribute<OrderAttribute>()?.Value ?? 0)
            .ToArray();

        private readonly IRpcHandler _rpcHandler;

        private readonly IRpcInvoker _innerInvoker;
        
        public HandledInvoker(IRpcHandler rpcHandler, IRpcInvoker innerInvoker)
        {
            _rpcHandler = rpcHandler;
            _innerInvoker = innerInvoker;
        }

        public static IRpcInvoker For(IRpcInvoker invoker)
        {
            foreach (var handlerType in HandlerTypes)
            {
                invoker = new HandledInvoker((IRpcHandler) Activator.CreateInstance(handlerType), invoker);
            }

            return invoker;
        }

        public bool IsConsumerSide => _innerInvoker.IsConsumerSide;

        public Uri Uri => _innerInvoker.Uri;
        
        public IRpcResult Invoke(IRpcInvocation invocation)
        {
            try
            {
                return _rpcHandler.Invoke(_innerInvoker, invocation);
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