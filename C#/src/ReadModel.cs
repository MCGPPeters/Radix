using System;
using System.Collections.Generic;
using System.Reactive.Disposables;
using System.Threading.Tasks;

namespace Radix
{
    public class
        ReadModel<TState, TEvent> : IObserver<TEvent>, IEquatable<ReadModel<TState, TEvent>>
        where TEvent : Event
        where TState : State<TState, TEvent>, IEquatable<TState>, new()
    {

        private IObserver<TState>? _observer;

        private ReadModel(TState state) => State = state;

        public TState State { get; private set; }

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

        public void OnNext(TEvent @event)
        {
            State = State.Apply(@event);
            if (_observer is object)
            {
                _observer.OnNext(State);
            }
        }

        public static async Task<ReadModel<TState, TEvent>> Create(IAsyncEnumerable<TEvent> history) => new ReadModel<TState, TEvent>(await Initial<TState, TEvent>.State(history));

        public IDisposable Subscribe(IObserver<TState> observer)
        {
            _observer = observer;
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
