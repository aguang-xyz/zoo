namespace Zoo.Rpc.Abstractions.Nodes
{
    /// <summary>
    /// RPC exporter.
    /// </summary>
    public interface IRpcExporter : IRpcNode
    {
        /// <summary>
        /// Start.
        /// </summary>
        void Start();
    }
}