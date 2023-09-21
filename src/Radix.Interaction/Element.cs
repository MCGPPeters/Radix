using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Radix.Math.Pure.Logic;

namespace Radix.Interaction;

public static class Element
{
    public static Data.Node Create<T>(Data.Attribute[] attributes, Data.Node[] children,
        [CallerLineNumber] int nodeId = 0) where T : Literal<T>, ElementName
        => (component, builder)
            =>
        {
            builder.OpenRegion(nodeId);
            builder.OpenElement(nodeId, T.Format());

            foreach (Data.Attribute attribute in attributes)
            {
                attribute()(component, builder);
            }

            foreach (Data.Node child in children)
            {
                child(component, builder);
            }

//builder.AddElementReferenceCapture(int.MaxValue,
//    elementReference => element.ElementReference = elementReference);

            builder.CloseElement();
            builder.CloseRegion();
        };
}
