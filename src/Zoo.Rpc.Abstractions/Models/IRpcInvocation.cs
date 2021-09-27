using System;
using System.Collections.Generic;

namespace Zoo.Rpc.Abstractions.Models
{
    /// <summary>
    /// RPC invocation.
    /// </summary>
    public interface IRpcInvocation
    {
        /// <summary>
        /// Service type.
        /// </summary>
        public Type ServiceType { get; }
        
        /// <summary>
        /// Method name.
        /// </summary>
        public string MethodName { get; }
        
        /// <summary>
        /// Parameter types.
        /// </summary>
        public Type[] ParameterTypes { get; }
        
        /// <summary>
        /// Parameters.
        /// </summary>
        public object[] Parameters { get; }
        
        /// <summary>
        /// Additional attachments.
        /// </summary>
        public IDictionary<string, string> Attachments { get; }
    }
}