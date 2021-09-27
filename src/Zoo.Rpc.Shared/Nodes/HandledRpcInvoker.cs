using System;
using Zoo.Rpc.Abstractions.Handlers;
using Zoo.Rpc.Abstractions.Models;
using Zoo.Rpc.Abstractions.Nodes;

namespace Zoo.Rpc.Shared.Nodes
{
    /// <summary>
    /// Invoker with some handler.
    /// </summary>
    public class HandledRpcInvoker : IRpcInvoker
    {
        private readonly IRpcHandler _handler;

        private readonly IRpcInvoker _innerInvoker;
        
        public HandledRpcInvoker(IRpcHandler handler, IRpcInvoker innerInvoker)
        {
            _handler = handler;
            _innerInvoker = innerInvoker;
        }

        public bool IsConsumerSide => _innerInvoker.IsConsumerSide;

        public Uri Uri => _innerInvoker.Uri;
        
        public IRpcResult Invoke(IRpcInvocation invocation)
        {
            return _handler.Invoke(_innerInvoker, invocation);
        }
        
        public void Dispose()
        {
        }
    }
}