using System;
using System.Collections.Generic;
using Zoo.Rpc.Abstractions.Models;

namespace Zoo.Rpc.Shared.Models
{
    /// <summary>
    /// Default RPC result.
    /// </summary>
    public class DefaultRpcResult : IRpcResult
    {
        public object ReturnValue { get; set; }
        
        public Exception Exception { get; set; }
        
        public IDictionary<string, string> Attachments { get; set; }
    }
}