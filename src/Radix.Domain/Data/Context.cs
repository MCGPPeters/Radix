﻿using Radix.Control.Validated;
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
    /// <typeparam name="">The specialized aggregate command type</typeparam>
    /// <typeparam name="">The specialized aggregate command type</typeparam>
    /// <returns></returns>
    public async Task<Instance<TState, TCommand, TEvent>> Create<TState>()
        where TState : Aggregate<TState, TCommand, TEvent>
        =>
            await Get<TState>(this, new Address { Id = (Id)Guid.NewGuid(), TenantId = (TenantId)"" }, new MinimumVersion());

    
    /// <summary>
    /// Create a new instance of an aggregate, where the aggregate uses top level context commands and events
    /// </summary>
    /// <param name="aggregateId">By default an new Address will be generated, but you can pass a predefined one</param>
    /// <typeparam name="TState">The aggregate type</typeparam>
    /// <returns></returns>
    public async Task<Instance<TState, TCommand, TEvent>> Create<TState>(TenantId tentantId)
        where TState : Aggregate<TState, TCommand, TEvent>
        =>
            await Get<TState>(this, new Address{ Id = (Id)Guid.NewGuid(), TenantId = tentantId}, new MinimumVersion());

    public Task<Instance<TState, TCommand, TEvent>> Get<TState>(Address instanceAddress)
        where TState : Aggregate<TState, TCommand, TEvent>
        => Get<TState>(this, instanceAddress, new AnyVersion());

    public async Task<Instance<TState, TCommand,  TEvent>> Get<TState>(
        Context<TCommand, TEvent, TEventStore, TEventStoreSettings> context, Address instanceAddress, Version version)
        where TState : Aggregate<TState, TCommand, TEvent>

    {
        var state = TState.Create();
        var stream = new Stream { Id = (StreamId)instanceAddress.Id.ToString(), Name = (StreamName)TState.Id };
        var events = TEventStore.GetEvents<TEvent>(EventStoreSettings, instanceAddress.TenantId, stream, new Closed<Version>(new MinimumVersion(), version));
        List<TEvent> history = new();
        state = await events.AggregateAsync(state, (current, @event) =>
        {
            history.Add(@event.Value);
            return TState.Apply(current, @event.Value);
        });

        var instance = new Instance<TState, TCommand,  TEvent> { Address = instanceAddress, State = state, Version = version, History = history};
        instance.Handle =  command =>
        {
            return command
                .Select(async cmd => (await Handle(instance, cmd)))
                .Traverse(id => id);
        };
        return instance;
    }

    [Pure]
    internal async Task<Instance<TState, TCommand,  TEvent>> Handle<TState>(Instance<TState, TCommand, TEvent> instance, TCommand command)
        where TState : Aggregate<TState, TCommand, TEvent>
    {
        var stream = new Stream { Id = (StreamId)instance.Address.Id.ToString(), Name = (StreamName)TState.Id };
        var ourEvents = TState.Decide(instance.State, command);
        var theirEvents = TEventStore
            .GetEvents<TEvent>(EventStoreSettings, instance.Address.TenantId, stream, new Closed<Version>(instance.Version, new MaximumVersion()))
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
                    var i = instance with
                    {
                        State = newState, Version = version, History = instance.History.Concat(eventsToAppend),
                    };
                    i.Handle = command =>
                    {
                        return command
                            .Select(async cmd => (await Handle(i, cmd)))
                            .Traverse(id => id);
                    };
                    return i;
                }
            default:
                throw new NotSupportedException();
        }
    }
}

