using System;
using System.Collections.Generic;
using Refit;
using Zoo.Protocol.AspNetCore.Apis;
using Zoo.Rpc.Abstractions.Models;
using Zoo.Rpc.Abstractions.Nodes;
using Zoo.Rpc.Shared.Models;

namespace Zoo.Protocol.AspNetCore.Nodes
{
    public class RpcViaHttpInvoker : IRpcInvoker
    {
        private readonly string _hostUri;

        public RpcViaHttpInvoker(Type serviceType, Uri serviceUri)
        {
            _hostUri = $"{serviceUri.Scheme}://{serviceUri.Host}:{serviceUri.Port}/${serviceType.FullName}";
            Uri = serviceUri;
        }

        public Uri Uri { get; }

        public bool IsConsumerSide => true;
        
        private IRpcApi Api => RestService.For<IRpcApi>(_hostUri);
        
        public IRpcResult Invoke(IRpcInvocation invocation)
        {
            try
            {
                return Api.Invoke(new DefaultRpcInvocation(invocation)).Result;
            }
            catch (Exception e)
            {
                return new DefaultRpcResult
                {
                    Exception = e,
                    Attachments = new Dictionary<string, string>()
                };
            }
        }
        
        public void Dispose()
        {
        }
    }
}