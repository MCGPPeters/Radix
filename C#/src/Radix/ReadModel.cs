using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Threading.Tasks;

namespace Radix
{
    public class
        ReadModel<TState, TEvent> : IEquatable<ReadModel<TState, TEvent>>
        where TState : new() where TEvent : Event
    {

        private ReadModel(TState state) => State = state;

        public TState State { get; }

        public bool Equals(ReadModel<TState, TEvent>? other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return State.Equals(other.State);
        }

        public void OnCompleted()
        {

        }

        public void OnError(Exception error)
        {

        }

        public static async Task<ReadModel<TState, TEvent>> Create(IAsyncEnumerable<TEvent> history, Update<TState, TEvent> update)
        {
            TState state = new TState();

            // restore the initialState (if any)
            if (history is object)
            {
                state = await history.AggregateAsync(state, (s, @event) =>  update(s, @event));
            }

            return new ReadModel<TState, TEvent>(state);
        }

        public IDisposable Subscribe(IObserver<TState> observer)
        {
            return Disposable.Empty;
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj.GetType() != GetType())
            {
                return false;
            }

            return Equals((ReadModel<TState, TEvent>)obj);
        }

        public override int GetHashCode() => State.GetHashCode();

        public static bool operator ==(ReadModel<TState, TEvent>? left, ReadModel<TState, TEvent>? right) => Equals(left, right);

        public static bool operator !=(ReadModel<TState, TEvent>? left, ReadModel<TState, TEvent>? right) => !Equals(left, right);
    }
}
