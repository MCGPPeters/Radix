namespace Radix;

public delegate TState Update<TState, in TEvent>(TState state, params TEvent[] @event);
