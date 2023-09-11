using System.Runtime.CompilerServices;
using Radix.Interaction.Components.Nodes;
using Radix.Interaction.Data;

namespace Radix.Interaction.Components;

public static class Prelude
{
    public static Node text(string text, [CallerLineNumber] int nodeId = 0) => new Text(text, nodeId);

    public static Concat concat( Node[] nodes, [CallerLineNumber] int nodeId = 0) => new((NodeId)nodeId, nodes);
}
