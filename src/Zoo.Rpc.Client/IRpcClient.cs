using System;

namespace Zoo.Rpc.Client
{
    /// <summary>
    /// Rpc client.
    /// </summary>
    public interface IRpcClient : IDisposable
    {
        void Provide<TService>(object service);

        void Provide(Type serviceType, object service);

        TService Consume<TService>();

        object Consume(Type serviceType);

        public void Start();
    }
}