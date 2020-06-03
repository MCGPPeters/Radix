namespace Radix
{
    public delegate TState Update<TState, in TEvent>(TState state, TEvent @event)
        where TEvent : Event;
}
