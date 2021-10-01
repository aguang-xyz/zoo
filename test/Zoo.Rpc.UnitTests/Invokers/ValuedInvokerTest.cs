using System;
using FluentAssertions;
using Xunit;
using Zoo.Rpc.Core.Invokers;
using Zoo.Rpc.Core.Models;
using Zoo.Rpc.UnitTests.Support;

namespace Zoo.Rpc.UnitTests.Invokers
{
    public class ValuedInvokerTest
    {
        [Theory]
        [ClassData(typeof(InvokerDataGenerator))]
        public void Invoke(Uri uri, bool isConsumerSide)
        {
            // Given
            var expectedResult = new RpcResult();
            var invoker = new ValuedInvoker(uri, isConsumerSide, expectedResult);
            
            // When
            var result = invoker.Invoke(new RpcInvocation());
            
            // Then
            invoker.Uri.Should().Be(uri);
            invoker.IsConsumerSide.Should().Be(isConsumerSide);
            result.Should().Be(expectedResult);
        }
    }
}