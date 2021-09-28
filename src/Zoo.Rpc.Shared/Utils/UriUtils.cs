using System;
using System.Collections.Specialized;
using System.Reflection;
using System.Web;
using Zoo.Rpc.Abstractions.Attributes;
using Zoo.Rpc.Abstractions.Constants;

namespace Zoo.Rpc.Shared.Utils
{
    /// <summary>
    /// URI utils.
    /// </summary>
    public static class UriUtils
    {
        private static NameValueCollection BuildDefaultQuery(Type serviceType, Uri serviceUri)
        {
            var query = HttpUtility.ParseQueryString(serviceUri.Query);

            // Add service version.
            query.Add(ParameterNames.ServiceVersion, 
                serviceType.GetCustomAttribute<VersionAttribute>()?.Value ?? DefaultValues.ServiceVersion);

            return query;
        }
        
        /// <summary>
        /// Create consumer URI.
        /// </summary>
        /// <param name="serviceType"></param>
        /// <param name="serviceUri"></param>
        /// <returns></returns>
        public static Uri CreateConsumerUri(Type serviceType, Uri serviceUri)
        {
            var query = BuildDefaultQuery(serviceType, serviceUri);
            
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
            var query = BuildDefaultQuery(serviceType, serviceUri);
            
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