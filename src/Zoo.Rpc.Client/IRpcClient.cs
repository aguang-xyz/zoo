using System;
using Zoo.Rpc.Abstractions.Nodes;

namespace Zoo.Rpc.Client
{
    /// <summary>
    /// Rpc client.
    /// </summary>
    public interface IRpcClient : IDisposable
    {
        void Provide(Type serviceType, IRpcInvoker invoker);

        object Consume(Type serviceType);

        public void Start();
    }
}