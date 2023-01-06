using System.Runtime.CompilerServices;
using Radix.Interaction.Data;
using Attribute = Radix.Interaction.Data.Attribute;

namespace Radix.Interaction.Components.Nodes;

public record Component : Node
{
    public Component(Type type, IEnumerable<Attribute> attributes, IEnumerable<Node> children, [CallerLineNumber] int nodeId = 0)  : base((NodeId)nodeId)
    {
        Type = type;
        Attributes = attributes;
        Children = children;
    }

    public Component(Type type, IEnumerable<Attribute> attributes, [CallerLineNumber] int nodeId = 0) : base((NodeId)nodeId)
    {
        Type = type;
        Attributes = attributes;
    }

    public Type Type { get; }
    public IEnumerable<Attribute> Attributes { get; }
    public IEnumerable<Node> Children { get; } = new List<Node>();
}

public delegate Component component(Attribute[] attributes, Node[] children, [CallerLineNumber] int nodeId = 0);

public delegate Component component<in T>(params T[] attributes) where T : Attribute;
