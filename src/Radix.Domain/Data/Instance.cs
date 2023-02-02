using Version = Radix.Domain.Data.Version;

namespace Radix.Domain.Data;

public record Instance<TState, TCommand, TAggregateCommand, TEvent,  TAggregateEvent, TEventStore>
    where TEventStore : EventStore<TEventStore>
    where TState : Aggregate<TState, TAggregateCommand, TAggregateEvent>, new()
    where TAggregateCommand : TCommand
    where TAggregateEvent : TEvent
{
    public required Aggregate.Id Id { get; init; }
    public required TState State { get; init; }
    public required Version Version { get; init; }

    public required IEnumerable<TAggregateEvent> History { get; init; }
    public required Context<TCommand, TEvent, TEventStore> Context { get; init; }
}
