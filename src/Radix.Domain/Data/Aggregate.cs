namespace Radix.Domain.Data;

public interface Aggregate<TState, in TCommand, TEvent>
    where TState : Aggregate<TState, TCommand, TEvent>
{
    static abstract TState Create();

    static virtual string Id => nameof(TState);

    static abstract TState Apply(TState state, TEvent @event);

    static abstract IEnumerable<TEvent> Decide(TState state, TCommand command);

    /// <summary>
    /// Decide which events need to be emitted that will resolve the conflicting situation.
    ///
    /// This could potentially be an entire new sequence of events, a merger of the two sequences, the result of rebasing the new events on the existing events
    /// or even an empty sequence. Think resolving merge conflicts in git.
    /// </summary>
    /// <param name="state"></param>
    /// <param name="ourEvents"></param>
    /// <param name="theirEvents"></param>
    /// <returns></returns>
    static abstract IAsyncEnumerable<TEvent> ResolveConflicts(TState state, IEnumerable<TEvent> ourEvents, IOrderedAsyncEnumerable<Event<TEvent>> theirEvents);

}
