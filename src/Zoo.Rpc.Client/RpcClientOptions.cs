using System;

namespace Zoo.Rpc.Client
{
    /// <summary>
    /// RPC client options.
    /// </summary>
    public class RpcClientOptions
    {
        /// <summary>
        /// Register URI.
        /// </summary>
        public Uri RegistryUri { get; set; }
        
        /// <summary>
        /// Service URI.
        /// </summary>
        public Uri ServiceUri { get; set; }
    }
}