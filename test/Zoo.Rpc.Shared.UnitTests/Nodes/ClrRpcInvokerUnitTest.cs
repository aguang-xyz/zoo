using System;
using System.Collections.Generic;
using Xunit;
using Zoo.Rpc.Shared.Models;
using Zoo.Rpc.Shared.Nodes;
using FluentAssertions;
using Zoo.Rpc.Abstractions.Nodes;

namespace Zoo.Rpc.Shared.UnitTests.Nodes
{
    public class ClrRpcInvokerUnitTest
    {
        public interface IExampleService
        {
            string Echo(string message);

            string EchoFailed(string message);
        }

        public class ExampleService : IExampleService
        {
            public string Echo(string message)
            {
                return $"Hello, {message}";
            }

            public string EchoFailed(string message)
            {
                throw new Exception($"Failed, {message}");
            }
        }
        
        [Fact]
        public void Given_Successful_Invocation_Then_Result_Should_Contain_ReturnValue()
        {
            // Given
            IRpcInvoker invoker = new ClrRpcInvoker(new ExampleService());

            // When
            var result = invoker.Invoke(new DefaultRpcInvocation
            {
                ServiceTypeName = typeof(IExampleService).FullName,
                MethodName = nameof(IExampleService.Echo),
                ParameterTypeNames = new [] { typeof(string).FullName },
                Parameters = new object[] { "zoo" },
                Attachments = new Dictionary<string, string>()
            });

            // Then
            result.ReturnValue.Should().Be("Hello, zoo");
            result.Exception.Should().BeNull();
            result.Attachments.Should().BeEmpty();
        }
        
        [Fact]
        public void Given_Failed_Invocation_Then_Result_Should_Contain_Exception()
        {
            // Given
            IRpcInvoker invoker = new ClrRpcInvoker(new ExampleService());

            // When
            var result = invoker.Invoke(new DefaultRpcInvocation
            {
                ServiceTypeName = typeof(IExampleService).FullName,
                MethodName = nameof(IExampleService.EchoFailed),
                ParameterTypeNames = new [] { typeof(string).FullName },
                Parameters = new object[] { "zoo" },
                Attachments = new Dictionary<string, string>()
            });
            
            var exception = result.Exception;

            // Then
            result.ReturnValue.Should().BeNull();

            exception.GetType().Should().Be(typeof(Exception));
            exception.Message.Should().Be("Failed, zoo");
            
            result.Attachments.Should().BeEmpty();
        }
    }
}