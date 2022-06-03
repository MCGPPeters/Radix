namespace Radix;

public interface CommandHandler<TState, in TCommand, TEvent>
{
    static abstract Update<TState, TEvent> Update { get; }

    static abstract Decide<TState, TCommand, TEvent> Decide { get; }
}
