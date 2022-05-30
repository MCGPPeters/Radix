using Radix.Interaction.Data;

namespace Radix.Interaction.Web.Components.Nodes;

public record HtmlString(NodeId NodeId, string Value) : Node(NodeId);
