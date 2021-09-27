using System;

namespace Zoo.Rpc.Abstractions.Nodes
{
    /// <summary>
    /// RPC node, this is the basic abstraction of all entities which can be represented by a uri, including:
    /// 
    ///   * provider
    ///   * consumer
    ///   * exporter
    ///   * invoker
    ///   * registry
    /// 
    /// </summary>
    public interface IRpcNode : IDisposable
    {
        /// <summary>
        /// Uri representing the current node.
        ///
        /// Some examples:
        ///
        ///  * `consul://example.com:8500`: a `consul` registry.
        ///  * `mysql://example.com:3306`: a `mysql` registry.
        ///  * `redis://example.com:6379`: a `redis` registry.
        ///  * `http://example.com:8080/com.example.services.IHelloService?NodeType=Provider`:
        ///     a provider with `http` scheme serving `com.example.services.IHelloService`.
        ///  * `tcp://example.com:8080/com.example.services.IHelloService?NodeType=Consumer`:
        ///     a consumer with `tcp` scheme serving `com.example.services.IHelloService`.
        /// 
        /// </summary>
        Uri Uri { get; }
    }
}