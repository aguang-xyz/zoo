using System;
using System.Collections.Generic;

namespace Zoo.Rpc.Abstractions.Models
{
    /// <summary>
    /// RPC result.
    /// </summary>
    public interface IRpcResult
    {
        /// <summary>
        /// Return value.
        /// </summary>
        object ReturnValue { get; }
        
        /// <summary>
        /// Exception.
        /// </summary>
        Exception Exception { get; }
        
        /// <summary>
        /// Additional attachments.
        /// </summary>
        IDictionary<string, string> Attachments { get; }
    }
}