using Radix.Interaction.Components.Nodes;
using Radix.Interaction.Data;

namespace Radix.Interaction.Components;

public static class Prelude
{
    public static Node text(NodeId nodeId, string text) => new Text(nodeId, text);

    public static Concat concat(NodeId nodeId, params Node[] nodes) => new(nodeId, nodes);
}
