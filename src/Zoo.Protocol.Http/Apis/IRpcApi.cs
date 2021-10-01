using System.Threading.Tasks;
using Refit;
using Zoo.Rpc.Core.Models;

namespace Zoo.Protocol.Http.Apis
{
    public interface IRpcApi
    {
        [Headers("X-Zoo-Rpc: true")]
        [Post("/.zoo-rpc")]
        Task<RpcResult> Invoke([Body] RpcInvocation rpcInvocation);
    }
}