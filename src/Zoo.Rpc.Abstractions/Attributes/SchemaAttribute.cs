using System;

namespace Zoo.Rpc.Abstractions.Attributes
{
    /// <summary>
    /// Schema attribute, this will be used on top of exporter factory, invoker factory or registry factory. 
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class SchemaAttribute : Attribute
    {
        /// <summary>
        /// Schema attribute
        /// </summary>
        /// <param name="value">scheme</param>
        public SchemaAttribute(string value)
        {
            Value = value;
        }

        public string Value { get; }
    }
}