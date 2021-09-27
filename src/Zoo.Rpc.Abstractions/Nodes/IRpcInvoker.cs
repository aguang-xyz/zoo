using Zoo.Rpc.Abstractions.Models;

namespace Zoo.Rpc.Abstractions.Nodes
{
    /// <summary>
    /// RPC invoker.
    /// </summary>
    public interface IRpcInvoker : IRpcNode
    {
        /// <summary>
        /// Is invoking a remote service or local service. This property is usually used in handlers to support customized
        /// additional logic (for example: adding some tracing id into attachments from consumer side and restoring it on
        /// provider side).
        /// </summary>
        bool IsConsumerSide { get; }
        
        /// <summary>
        /// Invoke.
        /// </summary>
        /// <param name="invocation"></param>
        /// <returns></returns>
        IRpcResult Invoke(IRpcInvocation invocation);
    }
}