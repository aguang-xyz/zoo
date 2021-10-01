using Microsoft.AspNetCore.Builder;
using Zoo.Protocol.Http.Middlewares;

namespace Zoo.Protocol.Http.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static void UseRpc(this IApplicationBuilder applicationBuilder)
        {
            applicationBuilder.UseMiddleware<RpcMiddleware>();
        }
    }
}