using Microsoft.AspNetCore.Components.Rendering;

namespace Radix.Components.Html;

public class ExplicitAttribute : IAttribute
{
    public ExplicitAttribute(string name, Func<RenderTreeBuilder, int, object, int> factory)
    {
        Name = name;
        Factory = factory;
    }

    public Func<RenderTreeBuilder, int, object, int> Factory { get; }

    public string Name { get; set; }
}
