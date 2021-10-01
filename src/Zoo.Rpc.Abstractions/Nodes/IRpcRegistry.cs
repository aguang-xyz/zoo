using System;

namespace Zoo.Rpc.Abstractions.Nodes
{
    /// <summary>
    /// RPC registry.
    /// </summary>
    public interface IRpcRegistry : IRpcNode
    {
        /// <summary>
        /// Register.
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        void Register(Uri uri);

        /// <summary>
        /// Unregister.
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        void Unregister(Uri uri);

        /// <summary>
        /// Subscribe.
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="notify"></param>
        /// <returns></returns>
        void Subscribe(Uri uri, Action<Uri[]> notify);

        /// <summary>
        /// Unsubscribe.
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="notify"></param>
        /// <returns></returns>
        void Unsubscribe(Uri uri, Action<Uri[]> notify);

        /// <summary>
        /// Retrieve the full list of related uris from the registry server.
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        Uri[] Lookup(Uri uri);
    }
}