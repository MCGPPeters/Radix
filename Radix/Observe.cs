namespace Radix;

public delegate TCommand Observe<in TEvent, out TCommand>(TEvent[] @event);
