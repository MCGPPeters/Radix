using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Text;

namespace Radix.Blazor.Html
{
    public delegate Attribute Event(IEnumerable<NonNullString> values);

    public static class Events<T> where T : EventArgs
    {
        public static Attribute @event(Name name, Action<T> callback)
        {
            new Attribute(name, EventCallback.Factory.Create(name, callback));
        }
    }
}
