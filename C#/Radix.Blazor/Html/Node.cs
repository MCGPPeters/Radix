using System;
using System.Collections.Generic;

namespace Radix.Blazor.Html
{
    public interface Node
    {

    }

    public struct Component : Node
    {
        public Type Type { get; }
        public IEnumerable<Attribute> Attributes { get; }
        public IEnumerable<Node> Children { get; }

        public Component(Type type, IEnumerable<Attribute> attributes, IEnumerable<Node> children)

        {
            Type = type;
            Attributes = attributes;
            Children = children;

        }
    }
}