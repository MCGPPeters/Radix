using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Radix.Control.Result;
using Radix.Control.Validated;
using Radix.Data;
using Radix.Domain.Data;
using Radix.Math.Pure.Logic.Order.Intervals;
using Radix.Tests;
using Stream = Radix.Domain.Data.Stream;
using Version = System.Version;
using static Radix.Control.Validated.Extensions;
using static Radix.Control.Task.Validated.Extensions;

namespace Radix.Domain.Command;

/// <summary>
/// Type of a function the evaluates wetter or not a command can be executed given the state
/// </summary>
/// <typeparam name="TState"></typeparam>
/// <typeparam name="TCommand"></typeparam>
/// <param name="state"></param>
/// <returns></returns>
public delegate Validated<TCommand> Create<in TState, TCommand>(TState state);

/// <summary>
/// Type of a function that take a command and returns a validated version
/// </summary>
/// <typeparam name="TCommand">The type of the command</typeparam>
/// <param name="command">The command to validate</param>
/// <returns>A validated command</returns>
public delegate Validated<TCommand> Validate<TCommand>(TCommand command);

/// <summary>
/// Type of a function that takes the state and an event and return the new state
/// as a result of applying the event to the state
/// </summary>
/// <typeparam name="TState"></typeparam>
/// <typeparam name="TEvent"></typeparam>
/// <param name="state"></param>
/// <param name="event"></param>
/// <returns></returns>
public delegate TState Apply<TState, in TEvent>(TState state, TEvent @event);

/// <summary>
/// The type of a function that takes the state and generates a list of events as a consequence
/// of deciding if the command should be executed given the current state
/// </summary>
/// <typeparam name="TState"></typeparam>
/// <typeparam name="TCommand"></typeparam>
/// <typeparam name="TEvent"></typeparam>
/// <param name="state"></param>
/// <param name="command"></param>
/// <returns></returns>
public delegate IEnumerable<TEvent> Decide<in TState, in TCommand, out TEvent>(TState state, TCommand command);

/// <summary>
/// The type of a function that decides which events need to be emitted that will resolve a conflicting situation in case
/// of a optimistic concurrency error
///
/// This could potentially be an entire new sequence of events, a merger of the two sequences, the result of rebasing the new events on the existing events
/// or even an empty sequence. Think resolving merge conflicts in git.
/// </summary>
/// <param name="state">The state of the aggregate when the events were being applied</param>
/// <param name="ourEvents">The events created as a response to the command that led to the conflict</param>
/// <param name="theirEvents">The events that were added to the event stream that have a greater version
/// than the expected version at the moment of applying our events</param>
/// <returns></returns>
public delegate IAsyncEnumerable<TEvent> ResolveConflicts<in TState, TEvent>(TState state, IEnumerable<TEvent> ourEvents, IOrderedAsyncEnumerable<Event<TEvent>> theirEvents);

public delegate Task<Result<ExistingVersion, AppendEventsError>> AppendEvents<in TEvent>(TenantId tenantId, Data.Stream stream, Data.Version expectedVersion, params TEvent[] events);

public delegate IAsyncEnumerable<Event<TEvent>> GetEvents<TEvent>(TenantId tenantId, Data.Stream stream, Closed<Data.Version> interval);

public delegate Task PublishEvents<in TEvent>(TEvent[] events);

/// <summary>
/// Decide => GetEvents => 
/// </summary>
/// <typeparam name="TState"></typeparam>
/// <typeparam name="TCommand"></typeparam>
/// <typeparam name="TEvent"></typeparam>
/// <param name="state"></param>
/// <param name="command"></param>
/// <returns></returns>
public delegate Task<Validated<Instance<TState, TEvent>>> Handle<TState, in TCommand, TEvent>(Instance<TState, TEvent> aggregate, TCommand command)
    where TState : Aggregate<TState>;

public record struct Instance<TState, TEvent>(Guid Id, TenantId TenantId, TState State, Data.Version Version, IEnumerable<TEvent> History);

public interface Aggregate<TState>
    where TState : Aggregate<TState>

{
    static virtual string Id => typeof(TState).Name;
}

public static class Aggregate
{
    public static Handle<TState, TCommand, TEvent> Create<TState, TCommand, TEvent>(
        Validate<TCommand> validateCommand, Decide<TState, TCommand, TEvent> decide, Apply<TState, TEvent> apply, GetEvents<TEvent> getEvents,
        ResolveConflicts<TState, TEvent> resolveConflicts, AppendEvents<TEvent> appendEvents,
        PublishEvents<TEvent> publishEvents)
        where TState : Aggregate<TState> =>
        (instance, command) => Task.FromResult(validateCommand(command))
            .SelectMany(async cmd =>
            {
                var stream = new Stream { Id = (StreamId)instance.Id.ToString(), Name = (StreamName)TState.Id };
                var ourEvents = decide(instance.State, cmd);
                var theirEvents = getEvents(instance.TenantId, stream,
                        new Closed<Data.Version>(instance.Version, new MaximumVersion()))
                    .OrderBy(@event => @event.Version);
                var eventsToAppend = await resolveConflicts(instance.State, ourEvents, theirEvents).ToArrayAsync();
                var actualState =
                    await theirEvents.AggregateAsync(instance.State, (state, @event) => apply(state, @event.Value));
                Data.Version theirVersion = await theirEvents.MaxAsync(@event => @event.Version);
                var appendEventsResult = await appendEvents(instance.TenantId, stream,
                    theirVersion, eventsToAppend);
                switch (appendEventsResult)
                {
                    case Error<ExistingVersion, AppendEventsError>(var error):
                        return error switch
                        {
                            OptimisticConcurrencyError _ =>
                                // just retry, the conflict handling shall deal with this
                                await Create(validateCommand, decide, apply, getEvents,
                                resolveConflicts, appendEvents, publishEvents)(instance, command),
                            _ => throw new NotSupportedException()
                        };
                    case Ok<ExistingVersion, AppendEventsError>(var version):
                        await publishEvents(eventsToAppend);
                        return Valid(instance with
                        {
                            State = eventsToAppend.Aggregate(actualState, apply.Invoke),
                            Version = version,
                            History = instance.History.Concat(eventsToAppend),
                        });
                    default:
                        throw new NotSupportedException();
                }
            });

}
