namespace Radix;

public interface EventProcessor<TEvent, TCommand>
{
    static abstract Observe<TEvent, TCommand> Observe { get; }
}
