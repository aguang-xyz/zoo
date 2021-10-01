namespace Zoo.Rpc.Abstractions.Nodes
{
    /// <summary>
    /// RPC provider.
    /// </summary>
    public interface IRpcProvider : IRpcNode
    {
        /// <summary>
        /// Start.
        /// </summary>
        void Start();
    }
}