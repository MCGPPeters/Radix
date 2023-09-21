using Radix.Domain.Data;
using Radix.Domain.Data.Aggregate;
using Radix.Domain;

namespace Radix.Domain.Aggregate;

[Alias<Guid>]
public partial record Id
{

}

public delegate Task<Instance<TState, TCommand, TEvent>> Create<TState, TCommand, TEvent>(Id aggregateId)
    where TState : Aggregate<TState, TCommand, TEvent>;

public delegate Task<Instance<TState, TCommand, TEvent>> Get<TState, TCommand, TEvent>(Id aggregateId,
    Data.Version version)
    where TState : Aggregate<TState, TCommand, TEvent>;

public delegate Task<Instance<TState, TCommand, TEvent>> Handle<TState, TCommand, TEvent>(
    Instance<TState, TCommand, TEvent> instance, Command.Validate<TCommand> validate, TCommand command)
    where TState : Aggregate<TState, TCommand, TEvent>;

public delegate Handle<TState, TCommand, TEvent> ConfigureHandle<TState, TCommand, TEvent>(
    EventStore.GetEvents<TEvent> getEvents, EventStore.AppendEvents<TEvent> appendEvents)
    where TState : Aggregate<TState, TCommand, TEvent>;
