using System;

namespace Radix.Blazor
{
    public abstract class BoundedContextComponent<TViewModel, TCommand, TEvent> : Component<TViewModel, TCommand, TEvent>
        where TViewModel : State<TViewModel, TEvent>, IEquatable<TViewModel>, new() where TEvent : Event
    {
    }
}
