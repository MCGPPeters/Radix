using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.JSInterop;
using Radix.Interaction.Components;
using Radix.Interaction.Components.Nodes;
using Radix.Interaction.Data;
using Radix.Interaction.Web.Components.Nodes;
using Attribute = Radix.Interaction.Data.Attribute;

namespace Radix.Interaction.Web.Components;

public delegate Render Render<TModel, TCommand>(object currentComponent, RenderTreeBuilder builder);

public static class Prelude
{
    public static Func<object, RenderTreeBuilder, Render> Render { get; } =
        (object currentComponent, RenderTreeBuilder builder) =>
            node =>
            {
                switch (node)
                {

                    case Text(var id, var text):
                        builder.AddContent(id, text);
                        break;
                    case HtmlString(var id, var html):
                        builder.AddMarkupContent(id, html);
                        break;
                    case Concat nodes:
                        foreach (Node n in nodes)
                        {
                            Render(currentComponent, builder)(n);
                        }
                        break;
                    case Element element:
                        builder.OpenRegion(element.NodeId);
                        builder.OpenElement(element.NodeId, element.Name);

                        Attributes(currentComponent, builder, element.Attributes);
                        foreach (Node child in element.Children)
                        {
                            Render(currentComponent, builder)(child);
                        }

                        builder.AddElementReferenceCapture(int.MaxValue, __elementReference => element.ElementReference = __elementReference);

                        builder.CloseElement();
                        builder.CloseRegion();
                        break;
                    case Component component:
                        builder.OpenRegion(component.NodeId);
                        builder.OpenComponent(component.NodeId, component.Type);
                        Attributes(currentComponent, builder, component.Attributes);
                        if (component.Children.Any())
                        {
                            RenderFragment fragment = treeBuilder => treeBuilder.AddContent(
                                component.NodeId,
                                renderTreeBuilder =>
                                {
                                    foreach (Node elementChild in component.Children)
                                    {
                                        Render(currentComponent, renderTreeBuilder)(elementChild);
                                    }
                                });
                            builder.AddAttribute(1, "ChildContent", fragment);
                        }

                        builder.CloseComponent();
                        builder.CloseRegion();
                        break;
                }
            };

    private static void Attributes(object currentComponent, RenderTreeBuilder builder, IEnumerable<Data.Attribute> attributes)
    {
        foreach (Attribute attribute in attributes)
        {
            switch (attribute)
            {
                case ComponentAttribute(var id, var name, var value):
                    builder.AddAttribute(id, name, value);
                    break;
                case ExplicitAttribute explicitAttribute:
                    explicitAttribute.Factory(builder, explicitAttribute.NodeId, currentComponent);
                    break;
                case Data.Attribute<string> htmlAttribute:
                    if (htmlAttribute.Values.Any())
                    {
                        builder.AddAttribute(htmlAttribute.NodeId, htmlAttribute.Name, htmlAttribute.Values.Aggregate((current, next) => $"{current} {next}"));
                    }

                    break;
            }
        }
    }
}
