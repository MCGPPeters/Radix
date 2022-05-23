using Microsoft.AspNetCore.Components.Rendering;
using Radix.Interaction.Data;
using Attribute = Radix.Interaction.Data.Attribute;

namespace Radix.Interaction.Components;

public record ExplicitAttribute : Attribute
{
    public ExplicitAttribute(AttributeId attributeId, string name, Func<RenderTreeBuilder, int, object, int> factory) : base(name)
    {
        AttributeId = attributeId;
        Factory = factory;
    }

    public AttributeId AttributeId { get; }
    public Func<RenderTreeBuilder, int, object, int> Factory { get; }

}
