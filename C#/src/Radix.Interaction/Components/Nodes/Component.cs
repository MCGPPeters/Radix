using Radix.Interaction.Data;
using Attribute = Radix.Interaction.Data.Attribute;

namespace Radix.Interaction.Components.Nodes;

public record Component : Node
{
    public Component(NodeId nodeId, Type type, IEnumerable<Attribute> attributes, IEnumerable<Node> children) : base(nodeId)
    {
        Type = type;
        Attributes = attributes;
        Children = children;
    }

    public Component(NodeId nodeId, Type type, IEnumerable<Attribute> attributes) : base(nodeId)
    {
        NodeId = nodeId;
        Type = type;
        Attributes = attributes;
    }

    public Type Type { get; }
    public IEnumerable<Attribute> Attributes { get; }
    public IEnumerable<Node> Children { get; } = new List<Node>();
}

public delegate Component component(NodeId nodeId, Attribute[] attributes, params Node[] children);

public delegate Component component<in T>(params T[] attributes) where T : Attribute;
