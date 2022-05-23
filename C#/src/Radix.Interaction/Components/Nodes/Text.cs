using Radix.Interaction.Data;

namespace Radix.Interaction.Components.Nodes;

public record Text(NodeId NodeId, string Value) : Node(NodeId) { };
