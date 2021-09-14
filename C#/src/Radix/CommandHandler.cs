using System.Runtime.CompilerServices;

namespace Radix;

public interface CommandHandler<TState, TCommand, TEvent, TCommandHandler>
    where TCommandHandler : CommandHandler<TState, TCommand, TEvent, TCommandHandler>
{
    static abstract Update<TState, TEvent> Update { get; }

    static abstract Decide<TState, TCommand, TEvent> Decide { get; }
}
