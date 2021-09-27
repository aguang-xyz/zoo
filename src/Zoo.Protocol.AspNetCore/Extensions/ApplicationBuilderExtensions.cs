using Microsoft.AspNetCore.Builder;
using Zoo.Protocol.AspNetCore.Middlewares;

namespace Zoo.Protocol.AspNetCore.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static void UseRpc(this IApplicationBuilder applicationBuilder)
        {
            applicationBuilder.UseMiddleware<RpcMiddleware>();
        }
    }
}