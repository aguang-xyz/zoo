using System;
using System.Web;
using Zoo.Rpc.Abstractions.Constants;

namespace Zoo.Rpc.Shared.Utils
{
    /// <summary>
    /// URI utils.
    /// </summary>
    public static class UriUtils
    {
        /// <summary>
        /// Create consumer URI.
        /// </summary>
        /// <param name="serviceType"></param>
        /// <param name="serviceUri"></param>
        /// <returns></returns>
        public static Uri CreateConsumerUri(Type serviceType, Uri serviceUri)
        {
            var query = HttpUtility.ParseQueryString(serviceUri.Query);
            
            query.Add(ParameterNames.NodeType, NodeTypes.Consumer);
            
            var builder = new UriBuilder(serviceUri)
            {
                Path = serviceType.FullName!,
                Query = query.ToString()!
            };

            return builder.Uri;
        }
        
        /// <summary>
        /// Create provider URI.
        /// </summary>
        /// <param name="serviceType"></param>
        /// <param name="serviceUri"></param>
        /// <returns></returns>
        public static Uri CreateProviderUri(Type serviceType, Uri serviceUri)
        {
            var query = HttpUtility.ParseQueryString(serviceUri.Query);
            
            query.Add(ParameterNames.NodeType, NodeTypes.Provider);
            
            var builder = new UriBuilder(serviceUri)
            {
                Path = serviceType.FullName!,
                Query = query.ToString()!
            };

            return builder.Uri;
        }
    }
}