using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Components;
using Radix.Interaction.Data;

namespace Radix.Interaction;

public static class Component
{

    public static Data.Node Create<T>(Data.Attribute[] attributes, Node[] children, [CallerLineNumber] int nodeId = 0)
        where T : IComponent 
            => (component, builder) =>

            {
                builder.OpenRegion(nodeId);
                builder.OpenComponent(nodeId, typeof(T));
                foreach (Data.Attribute attribute in attributes)
                {
                    attribute()(component, builder);
                }

                if (children.Length > 0)
                {
                    RenderFragment fragment = treeBuilder => treeBuilder.AddContent(
                        nodeId,
                        renderTreeBuilder =>
                        {
                            foreach (Node elementChild in children)
                            {
                                elementChild(component, renderTreeBuilder);
                            }
                        });
                    builder.AddAttribute(1, "ChildContent", fragment);
                }

                builder.CloseComponent();
                builder.CloseRegion();
            };
}
