namespace Zoo.Rpc.Abstractions.Nodes
{
    /// <summary>
    /// RPC Provider.
    /// </summary>
    public interface IRpcProvider : IRpcNode
    {
        /// <summary>
        /// Start.
        /// </summary>
        void Start();
    }
}