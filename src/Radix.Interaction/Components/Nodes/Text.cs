using System.Runtime.CompilerServices;
using Radix.Interaction.Data;

namespace Radix.Interaction.Components.Nodes;

public record Text(string Value, [CallerLineNumber] int nodeId = 0) : Node((NodeId)nodeId)
{
    public static implicit operator Text(string s) => new Text(s);
};
