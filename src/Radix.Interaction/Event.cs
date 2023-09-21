using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
namespace Radix.Interaction;

public static class Event
{
    public static Data.Event Create<T>()
        where T : Literal<T>, EventName =>
        (Action<EventArgs> callback, [CallerLineNumber] int nodeId = 0) =>
        () => (component, builder) =>
                builder.AddAttribute(nodeId, "on" + T.Format(), EventCallback.Factory.Create(component, callback));
}
