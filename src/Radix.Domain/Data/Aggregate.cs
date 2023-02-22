using Radix.Data;
using System.Security.Claims;

namespace Radix.Domain.Data;

public interface Aggregate<TState, in TCommand, TEvent>
    where TState : Aggregate<TState, TCommand, TEvent>
{
    static abstract TState Create();

    static virtual string Id => typeof(TState).Name;

    static abstract TState Apply(TState state, TEvent @event);

    static abstract IEnumerable<TEvent> Decide(TState state, TCommand command);

    /// <summary>
    /// Decide which events need to be emitted that will resolve a conflicting situation in case
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
    static abstract IAsyncEnumerable<TEvent> ResolveConflicts(TState state, IEnumerable<TEvent> ourEvents, IOrderedAsyncEnumerable<Event<TEvent>> theirEvents);

}
