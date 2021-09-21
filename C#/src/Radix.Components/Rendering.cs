using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Radix.Components.Html;

namespace Radix.Components;

public static class Rendering
{
    public static int RenderNode(object currentComponent, RenderTreeBuilder builder, int sequence, Node node)
    {
        switch (node)
        {
            case Empty _:
                return sequence;
            case Text text:
                builder.AddContent(sequence, text);
                return sequence + 1;
            case HtmlString html:
                builder.AddMarkupContent(sequence, html);
                return sequence + 1;
            case Concat nodes:
                foreach (Node n in nodes)
                {
                    RenderNode(currentComponent, builder, sequence, n);
                    sequence++;
                }

                return sequence;
            case Element element:
                builder.OpenElement(sequence, element.Name);
                sequence += 1;
                sequence = RenderAttributes(currentComponent, builder, sequence, element.Attributes);

                foreach (Node elementChild in element.Children)
                {
                    RenderNode(currentComponent, builder, sequence, elementChild);
                    sequence++;
                }

                builder.CloseElement();
                return sequence;
            case Component component:
                builder.OpenComponent(sequence, component.Type);
                sequence++;
                sequence = RenderAttributes(currentComponent, builder, sequence, component.Attributes);
                if (component.Children.Any())
                {
                    RenderFragment fragment = treeBuilder => treeBuilder.AddContent(
                        sequence++,
                        renderTreeBuilder =>
                        {
                            foreach (Node elementChild in component.Children)
                            {
                                RenderNode(currentComponent, renderTreeBuilder, sequence, elementChild);
                            }
                        });
                    builder.AddAttribute(sequence, "ChildContent", fragment);
                }

                builder.CloseComponent();
                sequence += component.Children.Any() ? 2 : 0;
                return sequence;
            default: return sequence;


        }
    }

    private static int RenderAttributes(object currentComponent, RenderTreeBuilder builder, int sequence, IEnumerable<IAttribute> attributes)
    {
        foreach (IAttribute attribute in attributes)
        {
            switch (attribute)
            {
                case Html.Attribute(var name, var values):
                    string[] attributeValues = values as string[] ?? values.ToArray();
                    if (attributeValues.Any())
                    {
                        builder.AddAttribute(sequence++, name, attributeValues.Aggregate((current, next) => $"{current} {next}"));
                    }

                    break;
                case ComponentAttribute(var name, var value):
                    builder.AddAttribute(sequence, name, value);
                    break;
                case ExplicitAttribute explicitAttribute:
                    sequence = explicitAttribute.Factory(builder, sequence, currentComponent);
                    break;
            }
        }

        return sequence;
    }
}
