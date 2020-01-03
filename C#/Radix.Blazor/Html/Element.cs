using Radix.Blazor.Html;
using System.Collections.Generic;

namespace Radix.Blazor
{
    public struct Element : Node 
    {
        public Element(Name name, IEnumerable<Attribute> attributes, IEnumerable<Node> children)
        {
            Name = name;
            Attributes = attributes;
            Children = children;
        }

        public Name Name { get; set; }
        public IEnumerable<Attribute> Attributes { get; set; }
        public IEnumerable<Node> Children { get; set; }

        
    }
}