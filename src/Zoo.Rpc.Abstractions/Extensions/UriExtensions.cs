using System;
using System.Web;
using Zoo.Rpc.Abstractions.Constants;

namespace Zoo.Rpc.Abstractions.Extensions
{
    public static class UriExtensions
    {
        public static string GetServiceName(this Uri uri)
        {
            var path = uri.AbsolutePath;
            return path[0] != '/' ? path: path[1..];
        }

        public static string GetServiceVersion(this Uri uri)
        {
            return uri.GetQueryParameter(ParameterNames.ServiceVersion) ?? DefaultValues.ServiceVersion;
        }

        public static string GetNodeType(this Uri uri)
        {
            return uri.GetQueryParameter(ParameterNames.NodeType);
        }

        public static string GetQueryParameter(this Uri uri, string name)
        {
            return HttpUtility.ParseQueryString(uri.Query)[name];
        }
    }
}