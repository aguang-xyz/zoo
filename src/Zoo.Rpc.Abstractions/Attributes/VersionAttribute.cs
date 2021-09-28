using System;

namespace Zoo.Rpc.Abstractions.Attributes
{
    /// <summary>
    /// Version attribute, this will be used on top of service interfaces.
    /// </summary>
    [AttributeUsage(AttributeTargets.Interface)]
    public class VersionAttribute : Attribute
    {
        public VersionAttribute(string value)
        {
            Value = value;
        }

        public string Value { get; }
    }
}