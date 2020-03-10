using System;
using System.Collections.Generic;

namespace Radix
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
    public interface Aggregate<out TState, TEvent, TCommand> : State<TState, TEvent> 
        where TState : IEquatable<TState>, new() 
        where TEvent : Event
    {
        /// <summary>
        ///     This is the place to validate (and log validation errors for instance of) a commandDescriptor and decide of any events will be generated as a
        ///     consequence of this commandDescriptor.
        ///     You MUST NOT change the state here
        ///     You MAY call external services
        /// </summary>
        /// <param name="commandDescriptor"></param>
        /// <returns></returns>
        TEvent[] Decide(CommandDescriptor<TCommand> commandDescriptor);
    }

}
