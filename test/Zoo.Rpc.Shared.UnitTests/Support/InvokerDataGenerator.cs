using System;
using System.Collections;
using System.Collections.Generic;

namespace Zoo.Rpc.Shared.UnitTests.Support
{
    public class InvokerDataGenerator : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            // HTTPS Provider.
            yield return new object[]
            {
                new Uri("https://example.com:8080/com.example.IExampleService?NodeType=Provider"),
                false
            };
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}