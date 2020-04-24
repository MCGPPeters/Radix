namespace Radix
{
    public interface EventStore<TEvent>
        where TEvent : Event
    {
        AppendEvents<TEvent> AppendEvents { get; }
        GetEventsSince<TEvent> GetEventsSince { get; }
    }
}
