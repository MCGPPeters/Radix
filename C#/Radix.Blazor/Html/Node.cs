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
        public IEnumerable<IAttribute> Attributes { get; }
        public IEnumerable<Node> Children { get; }

        public Component(Type type, IEnumerable<IAttribute> attributes, IEnumerable<Node> children)

        {
            Type = type;
            Attributes = attributes;
            Children = children;

        }
    }
}