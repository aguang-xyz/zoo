using System;
using FluentAssertions;
using Xunit;
using Zoo.Rpc.Core.Invokers;
using Zoo.Rpc.Core.Models;
using Zoo.Rpc.UnitTests.Support;

namespace Zoo.Rpc.UnitTests.Invokers
{
    public class UnavailableInvokerTest
    {
        [Theory]
        [ClassData(typeof(InvokerDataGenerator))]
        public void Invoke(Uri uri, bool isConsumerSide)
        {
            // Given
            var invoker = new UnavailableInvoker(uri, isConsumerSide);

            // When
            var result = invoker.Invoke(new RpcInvocation());
            var exception = result.Exception;
            
            // Then
            invoker.Uri.Should().Be(uri);
            invoker.IsConsumerSide.Should().Be(isConsumerSide);
            result.ReturnValue.Should().BeNull();
            exception.GetType().Should().Be(typeof(InvalidOperationException));
            exception.Message.Should().Be(UnavailableInvoker.Message);
            result.Attachments.Should().BeEmpty();
        }
    }
}