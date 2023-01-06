using System.Runtime.CompilerServices;
using Radix.Interaction.Data;

namespace Radix.Interaction.Web.Components.Nodes;

public record HtmlString(string Value, [CallerLineNumber] int nodeId = 0) : Node((NodeId)nodeId);
