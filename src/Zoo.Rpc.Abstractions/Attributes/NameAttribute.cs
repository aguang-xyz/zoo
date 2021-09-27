using System;

namespace Zoo.Rpc.Abstractions.Attributes
{
    /// <summary>
    /// Name attribute, this will be used on top of load balancer or handler definitions.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class NameAttribute : Attribute
    {
        /// <summary>
        /// Name attribute
        /// </summary>
        /// <param name="value">name of a handler or load balancer</param>
        public NameAttribute(string value)
        {
            Value = value;
        }

        public string Value { get; }
    }
}