namespace Zoo.Rpc.Abstractions.Nodes
{
    /// <summary>
    /// RPC consumer.
    /// </summary>
    public interface IRpcConsumer : IRpcNode
    {
        /// <summary>
        /// Start.
        /// </summary>
        void Start();

        /// <summary>
        /// Get dynamic proxy object.
        /// </summary>
        /// <returns></returns>
        object GetReference();
    }
}