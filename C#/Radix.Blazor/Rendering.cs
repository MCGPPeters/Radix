﻿using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Radix.Blazor.Html;

namespace Radix.Blazor
{
    public static class Rendering
    {
        public static int RenderNode(object currentComponent, RenderTreeBuilder builder, int sequence, Node node)
        {
            switch (node)
            {
                case Empty _:
                    return sequence;
                case Text text:
                    builder.AddContent(sequence, text.Value);
                    return sequence + 1;
                case HtmlString html:
                    builder.AddMarkupContent(sequence, html.Value);
                    return sequence + 1;
                case Concat nodes:
                    foreach (var n in nodes)
                    {
                        RenderNode(currentComponent, builder, sequence, n);
                        sequence++;
                    }

                    return sequence;
                case Element element:
                    builder.OpenElement(sequence, element.Name);
                    sequence += 1;
                    sequence = RenderAttributes(currentComponent, builder, sequence, element.Attributes);
                    foreach (var elementChild in element.Children)
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
                                foreach (var elementChild in component.Children)
                                {
                                    RenderNode(currentComponent, builder, sequence, elementChild);
                                    sequence++;
                                }
                            });
                        builder.AddAttribute(sequence, "ChildComponent", fragment);
                    }

                    builder.CloseComponent();
                    sequence += component.Children.Any() ? 2 : 0;
                    return sequence;
                default: return sequence;


            }
        }

        private static int RenderAttributes(object currentComponent, RenderTreeBuilder builder, int sequence, IEnumerable<IAttribute> attributes)
        {
            foreach (var attribute in attributes)
                switch (attribute)
                {
                    case Attribute (var name, var values):
                        builder.AddAttribute(sequence++, name, values.Aggregate((current, next) => current + " " + next));
                        break;
                    case ExplicitAttribute explicitAttribute:
                        sequence = explicitAttribute.Factory(builder, sequence, currentComponent);
                        break;
                }

            return sequence;
        }
    }
}
