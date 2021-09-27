using System;

namespace Zoo.Rpc.Abstractions.Attributes
{
    /// <summary>
    /// Order attribute, this will be used on top of a handler definition.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class OrderAttribute : Attribute
    {
        /// <summary>
        /// Order attribute.
        /// </summary>
        /// <param name="value">the order of a handler</param>
        public OrderAttribute(int value)
        {
            Value = value;
        }

        public int Value { get; }
    }
}