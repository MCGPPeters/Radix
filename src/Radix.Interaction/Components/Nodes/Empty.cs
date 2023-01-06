using System.Runtime.CompilerServices;
using Radix.Interaction.Data;

namespace Radix.Interaction.Components.Nodes;

public record Empty : Node
{
    public Empty([CallerLineNumber] int nodeId = 0) : base((NodeId)nodeId)
    {
        
    }
} 
