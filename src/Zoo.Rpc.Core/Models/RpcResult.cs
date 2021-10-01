using System;
using System.Collections.Generic;
using Zoo.Rpc.Abstractions.Models;

namespace Zoo.Rpc.Core.Models
{
    /// <summary>
    /// RPC result.
    /// </summary>
    public class RpcResult : IRpcResult
    {
        public object ReturnValue { get; set; }
        
        public Exception Exception { get; set; }
        
        public IDictionary<string, string> Attachments { get; set; }
    }
}