using System;

namespace Radix
{

    public interface State<out TState, in TEvent>
        where TEvent : Event
        where TState : IEquatable<TState>, new()
    {
        /// <summary>
        ///     Here the effect of the event on the state of the aggregate is determined.
        ///     You ONLY mutate the state here. You MUST NOT call any external services.
        ///     The new state will be returned as an effect
        /// </summary>
        public TState Apply(TEvent @event);
    }
}
