using System.Collections.Generic;

namespace Radix.Tests
{
    /// <summary>
    ///     The interface an Aggregate root must conform to
    /// </summary>
    /// <typeparam name="TState">
    ///     The type of the aggregate root.
    ///     The new() constraint is intended to enforce the ability to create an aggregate root with its initial values. Its
    ///     initial state.
    /// </typeparam>
    /// <typeparam name="TEvent">The type of events the aggregate root generates</typeparam>
    /// <typeparam name="TCommand">The type of commands the aggregate root accepts</typeparam>
    public interface Aggregate<out TState, TEvent, in TCommand> where TState : new()
    {
        /// <summary>
        ///     This is the place to validate a command and decide of any events will be generated as a
        ///     consequence of this command. You should not and are not allowed to change the state here
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        List<TEvent> Decide(TCommand command);

        /// <summary>
        ///     Here the effect of the event on the state of the aggregate is determined.
        ///     The new state will be returned as an effect
        /// </summary>
        TState Apply(TEvent @event);
    }
}