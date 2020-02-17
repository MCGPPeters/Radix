using System;
using Radix.Blazor.Html;
using System.Collections.Generic;

namespace Radix.Blazor
{
    public struct Element : Node 
    {
        public Element(Name name, IEnumerable<IAttribute> attributes, IEnumerable<Node> children)
        {
            Name = name;
            Attributes = attributes;
            Children = children;
        }
        
        public Element(Name name, IEnumerable<IAttribute> attributes) : this(name, attributes, Array.Empty<Node>())
        {
        }

        public Name Name { get; set; }
        public IEnumerable<IAttribute> Attributes { get; set; }
        public IEnumerable<Node> Children { get; set; }

    }
}