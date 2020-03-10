using System;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Radix.Blazor.Html;

namespace Radix.Blazor
{
    public abstract class BoundedContextComponent<TViewModel, TCommand, TEvent> : Component<TViewModel, TCommand, TEvent> 
        where TViewModel : State<TViewModel, TEvent>, IEquatable<TViewModel>, new() where TEvent : Event
    {

    }
}
