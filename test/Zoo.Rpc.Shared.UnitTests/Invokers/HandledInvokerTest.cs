using System;
using FluentAssertions;
using Xunit;
using Zoo.Rpc.Abstractions.Handlers;
using Zoo.Rpc.Abstractions.Models;
using Zoo.Rpc.Abstractions.Nodes;
using Zoo.Rpc.Core.Invokers;
using Zoo.Rpc.Core.Models;
using Zoo.Rpc.Shared.UnitTests.Support;

namespace Zoo.Rpc.Shared.UnitTests.Invokers
{
    public class HandledInvokerTest
    {
        private const string Message = "message";
        
        private class SuccessfulRpcHandler : IRpcHandler
        {
            public IRpcResult Invoke(IRpcInvoker invoker, IRpcInvocation invocation)
            {
                return invoker.Invoke(invocation);
            }
        }
        
        private class FailedRpcHandler : IRpcHandler
        {
            public IRpcResult Invoke(IRpcInvoker invoker, IRpcInvocation invocation)
            {
                throw new Exception(Message);
            }
        }

        [Theory]
        [ClassData(typeof(InvokerDataGenerator))]
        public void Invoke_Success(Uri uri, bool isConsumerSide)
        {
            // Given
            IRpcResult expectedRpcResult = new RpcResult();
            IRpcInvoker invoker = new HandledInvoker(new SuccessfulRpcHandler(),
                new ValuedInvoker(uri, isConsumerSide, expectedRpcResult));
            IRpcInvocation rpcInvocation = new RpcInvocation();
            
            // When
            var result = invoker.Invoke(rpcInvocation);
            
            // Then
            invoker.Uri.Should().Be(uri);
            invoker.IsConsumerSide.Should().Be(isConsumerSide);
            result.Should().Be(expectedRpcResult);
        }
        
        [Theory]
        [ClassData(typeof(InvokerDataGenerator))]
        public void Invoke_Failed(Uri uri, bool isConsumerSide)
        {
            // Given
            IRpcInvoker invoker = new HandledInvoker(new FailedRpcHandler(),
                new ValuedInvoker(uri, isConsumerSide, new RpcResult()));
            IRpcInvocation rpcInvocation = new RpcInvocation();
            
            // When
            var result = invoker.Invoke(rpcInvocation);
            var exception = result.Exception;
            
            // Then
            invoker.Uri.Should().Be(uri);
            invoker.IsConsumerSide.Should().Be(isConsumerSide);
            result.ReturnValue.Should().BeNull();
            exception.GetType().Should().Be(typeof(Exception));
            exception.Message.Should().Be(Message);
            result.Attachments.Should().BeEmpty();
        }
    }
}