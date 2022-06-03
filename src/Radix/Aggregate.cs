namespace Radix
{
    public interface Aggregate<TCommand, TEvent> where TEvent : notnull
    {
        Accept<TCommand, TEvent> Accept { get; }
        Id Id { get; }
    }
}
