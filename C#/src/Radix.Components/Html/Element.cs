using System;
using System.Collections.Generic;
using System.Linq;

namespace Radix.Components.Html
{
    public class Element : Node
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

        public Element(Name name, IEnumerable<Node> nodes) : this(name, Enumerable.Empty<IAttribute>(), nodes)
        {
        }

        public Name Name { get; }
        public IEnumerable<IAttribute> Attributes { get; }
        public IEnumerable<Node> Children { get; }
    }
}
