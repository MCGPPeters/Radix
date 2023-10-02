using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Rendering;
using Radix.Math.Pure.Logic;

namespace Radix.Interaction;

public static class Element
{
    public static Data.Node Create<T>(Data.Attribute[] attributes, Data.Node[] children, object? key = null,
        [CallerLineNumber] int nodeId = 0) where T : Literal<T>, ElementName
        => (component, builder)
            =>
        {
            builder.OpenElement(nodeId, T.Format());
            builder.SetKey(key);

            foreach (Data.Attribute attribute in attributes)
            {
                attribute(component, builder);
            }

            foreach (Data.Node child in children)
            {
                child(component, builder);
            }

            //builder.AddElementReferenceCapture(int.MaxValue,
            //    elementReference => element.ElementReference = elementReference);

            builder.CloseElement();
        };

    public static Data.Node Create<T>(Data.Node[] children, object? key = null, [CallerLineNumber] int nodeId = 0)
        where T : Literal<T>, ElementName =>
            Create<T>([], children, key, nodeId);

    public static Data.Node Create<T>(Data.Attribute[] attributes, object? key = null, [CallerLineNumber] int nodeId = 0)
        where T : Literal<T>, ElementName =>
            Create<T>(attributes, [], key, nodeId);
        
    public static void a(object component, RenderTreeBuilder builder)
    {
        builder.OpenElement(1, "a");
        builder.CloseElement();
    }
}
