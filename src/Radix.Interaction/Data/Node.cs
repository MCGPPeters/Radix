using Radix.Interaction.Components.Nodes;

namespace Radix.Interaction.Data;

public abstract record Node(NodeId NodeId)
{
    public static implicit operator Node(string s) => new Text(s);
}
