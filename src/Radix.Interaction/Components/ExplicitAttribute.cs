using Microsoft.AspNetCore.Components.Rendering;
using Radix.Interaction.Data;
using Attribute = Radix.Interaction.Data.Attribute;

namespace Radix.Interaction.Components;

public record ExplicitAttribute : Attribute
{
    public ExplicitAttribute(NodeId nodeId, string name, Func<RenderTreeBuilder, int, object, int> factory) : base(name)
    {
        NodeId = nodeId;
        Factory = factory;
    }

    public NodeId NodeId { get; }
    public Func<RenderTreeBuilder, int, object, int> Factory { get; }

}
