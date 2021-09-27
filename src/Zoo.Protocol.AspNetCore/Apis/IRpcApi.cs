using System.Threading.Tasks;
using Refit;
using Zoo.Rpc.Shared.Models;

namespace Zoo.Protocol.AspNetCore.Apis
{
    public interface IRpcApi
    {
        [Headers("X-Zoo-Rpc: true")]
        [Post("/.zoo-rpc")]
        Task<DefaultRpcResult> Invoke([Body] DefaultRpcInvocation invocation);
    }
}