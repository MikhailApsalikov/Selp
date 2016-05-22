namespace Selp.Common.Attributes
{
    using System;

    [AttributeUsage(AttributeTargets.Property)]
    public class PredicateAttribute : Attribute
    {
        public PredicateAttribute(string uri)
        {
            Uri = uri;
        }

        public string Uri { get; set; }
    }
}