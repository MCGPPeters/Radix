using Version = Radix.Domain.Data.Version;

namespace Radix.Domain.Data;

public record Instance<TState, TCommand, TAggregateCommand, TEvent,  TAggregateEvent, TEventStore, TEventStoreSettings>
    where TEventStore : EventStore<TEventStore, TEventStoreSettings>
    where TState : Aggregate<TState, TAggregateCommand, TAggregateEvent>
    where TAggregateCommand : TCommand
    where TAggregateEvent : TEvent
{
    public required Aggregate.Address Address { get; init; }
    public required TState State { get; init; }
    public required Version Version { get; init; }

    public required IEnumerable<TAggregateEvent> History { get; init; }
    public required Context<TCommand, TEvent, TEventStore, TEventStoreSettings> Context { get; init; }
}
