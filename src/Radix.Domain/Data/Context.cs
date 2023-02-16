using Radix.Data;
using Radix.Domain.Data.Aggregate;
using Radix.Math.Pure.Logic.Order.Intervals;
using Radix.Tests;
using System.Diagnostics.Contracts;

namespace Radix.Domain.Data;

/// <summary>
/// 
/// </summary>
/// <typeparam name="TCommand"></typeparam>
/// <typeparam name="TEvent"></typeparam>
/// <typeparam name="TEventStore"></typeparam>
/// <typeparam name="TEventStoreSettings"></typeparam>
public record Context<TCommand, TEvent, TEventStore, TEventStoreSettings>
    where TEventStore : EventStore<TEventStore, TEventStoreSettings>
{
    public required TEventStoreSettings EventStoreSettings { get; set; }

    /// <summary>
    /// Create a new instance of an aggregate, where the aggregate uses specialized events and commands within the context for the aggregate
    /// </summary>
    /// <param name="aggregateId">By default an new Address will be generated, but you can pass a predefined one</param>
    /// <typeparam name="TState">The aggregate type</typeparam>
    /// <typeparam name="TAggregateCommand">The specialized aggregate command type</typeparam>
    /// <typeparam name="TAggregateEvent">The specialized aggregate command type</typeparam>
    /// <returns></returns>
    public async Task<Instance<TState, TCommand, TAggregateCommand, TEvent, TAggregateEvent, TEventStore, TEventStoreSettings>> Create<TState, TAggregateCommand, TAggregateEvent>()
        where TState : Aggregate<TState, TAggregateCommand, TAggregateEvent>
        where TAggregateCommand : TCommand
        where TAggregateEvent : TEvent
        =>
            await Get<TState, TAggregateCommand, TAggregateEvent>(this, new Address { Id = (Id)Guid.NewGuid(), TenantId = (TenantId)"" }, new MinimumVersion());

    /// <summary>
    /// Create a new instance of an aggregate, where the aggregate uses top level context commands and events
    /// </summary>
    /// <param name="aggregateId">By default an new Address will be generated, but you can pass a predefined one</param>
    /// <typeparam name="TState">The aggregate type</typeparam>
    /// <returns></returns>
    public async Task<Instance<TState, TCommand, TCommand, TEvent, TEvent, TEventStore, TEventStoreSettings>> Create<TState>()
        where TState : Aggregate<TState, TCommand, TEvent>
        =>
            await Create<TState>((TenantId)"");
    
    /// <summary>
    /// Create a new instance of an aggregate, where the aggregate uses top level context commands and events
    /// </summary>
    /// <param name="aggregateId">By default an new Address will be generated, but you can pass a predefined one</param>
    /// <typeparam name="TState">The aggregate type</typeparam>
    /// <returns></returns>
    public async Task<Instance<TState, TCommand, TCommand, TEvent, TEvent, TEventStore, TEventStoreSettings>> Create<TState>(TenantId tentantId)
        where TState : Aggregate<TState, TCommand, TEvent>
        =>
            await Get<TState, TCommand, TEvent>(this, new Address{ Id = (Id)Guid.NewGuid(), TenantId = tentantId}, new MinimumVersion());

    public Task<Instance<TState, TCommand, TAggregateCommand, TEvent, TAggregateEvent, TEventStore, TEventStoreSettings>> Get<TState, TAggregateCommand, TAggregateEvent>(Address instanceAddress)
        where TState : Aggregate<TState, TAggregateCommand, TAggregateEvent>
        where TAggregateCommand : TCommand
        where TAggregateEvent : TEvent
        => Get<TState, TAggregateCommand, TAggregateEvent>(this, instanceAddress, new AnyVersion());

    public async Task<Instance<TState, TCommand, TAggregateCommand, TEvent, TAggregateEvent, TEventStore, TEventStoreSettings>> Get<TState, TAggregateCommand, TAggregateEvent>(
        Context<TCommand, TEvent, TEventStore, TEventStoreSettings> context, Address instanceAddress, Version version)
        where TState : Aggregate<TState, TAggregateCommand, TAggregateEvent>
        where TAggregateCommand : TCommand
        where TAggregateEvent : TEvent

    {
        var state = TState.Create();
        var stream = new Stream { Id = (StreamId)instanceAddress.Id.ToString(), Name = (StreamName)TState.Id };
        var events = TEventStore.GetEvents<TAggregateEvent>(EventStoreSettings, instanceAddress.TenantId, stream, new Closed<Version>(new MinimumVersion(), version));
        List<TAggregateEvent> history = new();
        state = await events.AggregateAsync(state, (current, @event) =>
        {
            history.Add(@event.Value);
            return TState.Apply(current, @event.Value);
        });

        return new Instance<TState, TCommand, TAggregateCommand, TEvent, TAggregateEvent, TEventStore, TEventStoreSettings> { Address = instanceAddress, State = state, Version = version, History = history, Context = context };
    }

    [Pure]
    internal async Task<Instance<TState, TCommand, TAggregateCommand, TEvent, TAggregateEvent, TEventStore, TEventStoreSettings>> Handle<TState, TAggregateCommand, TAggregateEvent>(Instance<TState, TCommand, TAggregateCommand, TEvent, TAggregateEvent, TEventStore, TEventStoreSettings> instance, TAggregateCommand command)
        where TState : Aggregate<TState, TAggregateCommand, TAggregateEvent>
        where TAggregateCommand : TCommand
        where TAggregateEvent : TEvent
    {
        var stream = new Stream { Id = (StreamId)instance.Address.Id.ToString(), Name = (StreamName)TState.Id };
        var ourEvents = TState.Decide(instance.State, command);
        var theirEvents = TEventStore
            .GetEvents<TAggregateEvent>(EventStoreSettings, instance.Address.TenantId, stream, new Closed<Version>(instance.Version, new MaximumVersion()))
            .OrderBy(@event => @event.Version);
        var eventsToAppend = await TState.ResolveConflicts(instance.State, ourEvents, theirEvents).ToArrayAsync();
        var actualState =
            await theirEvents.AggregateAsync(instance.State, (state, @event) => TState.Apply(state, @event.Value));
        var appendEventsResult = await TEventStore.AppendEvents(EventStoreSettings, instance.Address.TenantId, stream,
            await theirEvents.MaxAsync(@event => @event.Version), eventsToAppend);
        switch (appendEventsResult)
        {
            case Error<ExistingVersion, AppendEventsError>(var error):
                return error switch
                {
                    OptimisticConcurrencyError _ =>
                        // just retry, the conflict handling shall deal with this
                        await Handle(instance, command),
                    _ => throw new NotSupportedException()
                };
            case Ok<ExistingVersion, AppendEventsError>(var version):
                {
                    var newState = eventsToAppend.Aggregate(actualState, TState.Apply);
                    return instance with
                    {
                        State = newState,
                        Version = version,
                        History = instance.History.Concat(eventsToAppend)
                    };
                }
            default:
                throw new NotSupportedException();
        }
    }
}

