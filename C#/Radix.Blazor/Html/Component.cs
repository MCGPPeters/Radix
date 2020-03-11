using System;
using System.Collections.Generic;

namespace Radix.Blazor.Html
{
    public delegate Component component(IEnumerable<IAttribute> attributes, params Node[] children);

    public delegate Component component<in T>(params T[] attributes) where T : IAttribute;

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
