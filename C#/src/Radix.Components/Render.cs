using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Radix.Components.Html;

namespace Radix.Components;

public static class Render
{
    public static int Node(object currentComponent, RenderTreeBuilder builder, int sequence, Node node)
    {
        switch (node)
        {

            case Text (var text):
                builder.AddContent(sequence, text);
                return sequence + 1;
            case HtmlString (var html):
                builder.AddMarkupContent(sequence, html);
                return sequence + 1;
            case Concat nodes:
                foreach (Node n in nodes)
                {
                    Node(currentComponent, builder, sequence, n);
                    sequence++;
                }

                return sequence;
            case Element element:
                builder.OpenRegion(sequence++);
                int innerSequence = 0;
                builder.OpenElement(innerSequence, element.Name);
                
                sequence = Attributes(currentComponent, builder, sequence, element.Attributes);
                foreach (Node elementChild in element.Children)
                {
                    Node(currentComponent, builder, innerSequence, elementChild);
                    innerSequence++;
                }
                
                builder.AddElementReferenceCapture(int.MaxValue, __elementReference => element.ElementReference = __elementReference);
                
                builder.CloseElement();
                builder.CloseRegion();
                return sequence;
            case Html.Component component:
                builder.OpenComponent(sequence, component.Type);
                sequence++;
                sequence = Attributes(currentComponent, builder, sequence, component.Attributes);
                if (component.Children.Any())
                {
                    RenderFragment fragment = treeBuilder => treeBuilder.AddContent(
                        sequence++,
                        renderTreeBuilder =>
                        {
                            foreach (Node elementChild in component.Children)
                            {
                                Node(currentComponent, renderTreeBuilder, sequence, elementChild);
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

    private static int Attributes(object currentComponent, RenderTreeBuilder builder, int sequence, IEnumerable<IAttribute> attributes)
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
