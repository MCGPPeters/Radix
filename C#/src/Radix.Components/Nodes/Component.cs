using Microsoft.AspNetCore.Components;

namespace Radix.Components.Nodes;

public record Component : Node
{
    public Component(Type type, IEnumerable<Attribute> attributes, IEnumerable<Node> children)
    {
        Type = type;
        Attributes = attributes;
        Children = children;
    }

    public Component(Type type, IEnumerable<Attribute> attributes)
    {
        Type = type;
        Attributes = attributes;
    }

    public Type Type { get; }
    public IEnumerable<Attribute> Attributes { get; }
    public IEnumerable<Node> Children { get; } = new List<Node>();
}

public delegate Component component(Attribute[] attributes, params Node[] children);

public delegate Component component<in T>(params T[] attributes) where T : Attribute;
