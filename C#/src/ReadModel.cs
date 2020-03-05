using System;

namespace Radix
{
    public interface ReadModel<TState, in TEvent> : IObserver<TEvent>, IObservable<TState>, IEquatable<TState>
    {
        /// <summary>
        ///     Here the effect of the event on the state of the aggregate is determined.
        ///     You ONLY mutate the state here. You MUST NOT call any external services.
        ///     The new state will be returned as an effect
        /// </summary>
        void Apply(TEvent @event);
    }
}
