using System;
using System.Collections.Generic;
using Refit;
using Zoo.Protocol.Http.Apis;
using Zoo.Rpc.Abstractions.Models;
using Zoo.Rpc.Abstractions.Nodes;
using Zoo.Rpc.Core.Models;

namespace Zoo.Protocol.Http.Invokers
{
    public class HttpInvoker : IRpcInvoker
    {
        private readonly string _hostUri;

        public HttpInvoker(Type serviceType, Uri serviceUri)
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
                return Api.Invoke(new RpcInvocation(invocation)).Result;
            }
            catch (Exception e)
            {
                return new RpcResult
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