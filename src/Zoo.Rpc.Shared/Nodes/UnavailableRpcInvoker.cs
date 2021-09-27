using System;
using System.Collections.Generic;
using Zoo.Rpc.Abstractions.Models;
using Zoo.Rpc.Abstractions.Nodes;
using Zoo.Rpc.Shared.Models;

namespace Zoo.Rpc.Shared.Nodes
{
    public class UnavailableRpcInvoker : IRpcInvoker
    {
        public Uri Uri => throw new NotImplementedException();

        public bool IsConsumerSide => true;
        
        public IRpcResult Invoke(IRpcInvocation invocation)
        {
            return new DefaultRpcResult
            {
                Exception = new InvalidOperationException("service is not available"),
                Attachments = new Dictionary<string, string>()
            };
        }
        
        public void Dispose()
        {
        }
    }
}