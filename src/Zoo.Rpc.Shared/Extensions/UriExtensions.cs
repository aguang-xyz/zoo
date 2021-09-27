using System;
using System.Web;

namespace Zoo.Rpc.Shared.Extensions
{
    public static class UriExtensions
    {
        public static string GetServiceName(this Uri uri)
        {
            var path = uri.AbsolutePath;
            return path[0] != '/' ? path: path[1..];
        }

        public static string GetQueryParameter(this Uri uri, string name)
        {
            return HttpUtility.ParseQueryString(uri.Query)[name];
        }
    }
}