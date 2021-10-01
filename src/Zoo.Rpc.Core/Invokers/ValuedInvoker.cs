using System;
using Zoo.Rpc.Abstractions.Models;
using Zoo.Rpc.Abstractions.Nodes;

namespace Zoo.Rpc.Core.Invokers
{
    public class ValuedInvoker : IRpcInvoker
    {
        public ValuedInvoker(Uri uri, bool isConsumerSide, IRpcResult rpcResult)
        {
            Uri = uri;
            IsConsumerSide = isConsumerSide;
            RpcResult = rpcResult;
        }

        public Uri Uri { get; }
        
        public bool IsConsumerSide { get; }
        
        public IRpcResult RpcResult { get; }
        
        public IRpcResult Invoke(IRpcInvocation invocation)
        {
            return RpcResult;
        }
        
        public void Dispose()
        {
        }
    }
}