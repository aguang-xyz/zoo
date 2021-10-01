using System;
using System.Collections.Generic;
using FluentAssertions;
using Xunit;
using Zoo.Rpc.Abstractions.Models;
using Zoo.Rpc.Abstractions.Nodes;
using Zoo.Rpc.Core.Invokers;
using Zoo.Rpc.Core.Models;
using Zoo.Rpc.UnitTests.Support;

namespace Zoo.Rpc.UnitTests.Invokers
{
    public class ObjectInvokerTest
    {
        private const string Message = "message";

        private interface IExampleService
        {
            string Echo(string message);

            string EchoFailed(string message);
        }

        private class ExampleService : IExampleService
        {
            public string Echo(string message)
            {
                return message;
            }

            public string EchoFailed(string message)
            {
                throw new Exception(message);
            }
        }

        [Theory]
        [ClassData(typeof(InvokerDataGenerator))]
        public void Invoke_Success(Uri uri, bool isConsumerSide)
        {
            // Given
            IRpcInvoker invoker = new ObjectInvoker(uri, isConsumerSide, new ExampleService());
            IRpcInvocation rpcInvocation = new RpcInvocation
            {
                ServiceTypeName = typeof(IExampleService).FullName,
                MethodName = nameof(IExampleService.Echo),
                ParameterTypeNames = new[] { typeof(string).FullName },
                Parameters = new object[] { Message },
                Attachments = new Dictionary<string, string>()
            };

            // When
            var result = invoker.Invoke(rpcInvocation);

            // Then
            invoker.Uri.Should().Be(uri);
            invoker.IsConsumerSide.Should().Be(isConsumerSide);
            result.ReturnValue.Should().Be(Message);
            result.Exception.Should().BeNull();
            result.Attachments.Should().BeEmpty();
        }
        
        [Theory]
        [ClassData(typeof(InvokerDataGenerator))]
        public void Invoke_Failed(Uri uri, bool isConsumerSide)
        {
            // Given
            IRpcInvoker invoker = new ObjectInvoker(uri, isConsumerSide, new ExampleService());
            IRpcInvocation rpcInvocation = new RpcInvocation
            {
                ServiceTypeName = typeof(IExampleService).FullName,
                MethodName = nameof(IExampleService.EchoFailed),
                ParameterTypeNames = new[] { typeof(string).FullName },
                Parameters = new object[] { Message },
                Attachments = new Dictionary<string, string>()
            };
            
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