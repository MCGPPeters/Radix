using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.AspNetCore.Components;
namespace Radix.Interaction;

/// <summary>
/// Provides factory methods for creating events
/// </summary>
public static class Event
{
    public static Data.Attribute Create<T, TEventArgs>(Action<TEventArgs> callback, int nodeId = 0)
        where T : Literal<T>, EventName
        where TEventArgs : EventArgs =>

        (component, builder) =>
                builder.AddAttribute(nodeId, "on" + T.Format(), EventCallback.Factory.Create(component, callback));

    public static Data.Attribute Create<T>(Action<EventArgs> callback, int nodeId = 0)
        where T : Literal<T>, EventName =>
        (component, builder) =>
                builder.AddAttribute(nodeId, "on" + T.Format(), EventCallback.Factory.Create(component, callback));
}
