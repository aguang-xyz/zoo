using Zoo.Rpc.Abstractions.Models;
using Zoo.Rpc.Abstractions.Nodes;

namespace Zoo.Rpc.Abstractions.Handlers
{
    /// <summary>
    /// RPC handler, it will be used on both consumer side and provider side.
    ///
    /// We can use IInvoker.IsConsumerSide indicating if the current handler is executed on consumer side or provider side.
    /// </summary>
    public interface IRpcHandler
    {
        /// <summary>
        /// Invoke.
        /// </summary>
        /// <param name="invoker"></param>
        /// <param name="invocation"></param>
        /// <returns></returns>
        IRpcResult Invoke(IRpcInvoker invoker, IRpcInvocation invocation);
    }
}