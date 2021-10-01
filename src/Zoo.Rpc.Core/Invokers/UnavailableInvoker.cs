using System;
using System.Collections.Generic;
using Zoo.Rpc.Abstractions.Models;
using Zoo.Rpc.Abstractions.Nodes;
using Zoo.Rpc.Core.Models;

namespace Zoo.Rpc.Core.Invokers
{
    /// <summary>
    /// Unavailable invoker.
    /// </summary>
    public class UnavailableInvoker : IRpcInvoker
    {
        public const string Message = "service is not available";

        public UnavailableInvoker(Uri uri, bool isConsumerSide)
        {
            Uri = uri;
            IsConsumerSide = isConsumerSide;
        }
        
        public Uri Uri { get; }

        public bool IsConsumerSide { get; }
        
        public IRpcResult Invoke(IRpcInvocation invocation)
        {
            return new RpcResult
            {
                Exception = new InvalidOperationException(Message),
                Attachments = new Dictionary<string, string>()
            };
        }
        
        public void Dispose()
        {
        }
    }
}