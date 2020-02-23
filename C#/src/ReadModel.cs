using System;

namespace Radix
{
    public interface ReadModel<TReadModel, in TEvent> : IObservable<TReadModel>, IEquatable<TReadModel>
    {
        /// <summary>
        ///     Here the effect of the event on the state of the aggregate is determined.
        ///     You ONLY mutate the state here. You MUST NOT call any external services.
        ///     The new state will be returned as an effect
        /// </summary>
        void Apply(TEvent @event);
    }
}
