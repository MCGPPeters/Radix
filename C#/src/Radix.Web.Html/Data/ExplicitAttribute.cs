using Microsoft.AspNetCore.Components.Rendering;

namespace Radix.Web.Html.Data;

public class ExplicitAttribute : Attribute
{
    public ExplicitAttribute(string name, Func<RenderTreeBuilder, int, object, int> factory) : base(name, Enumerable.Empty<string>() )
    {
        Name = name;
        Factory = factory;
    }

    public Func<RenderTreeBuilder, int, object, int> Factory { get; }

    public string Name { get; init; }
}
