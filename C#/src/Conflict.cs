using System.Collections.Generic;

namespace Radix
{
    public struct Conflict<TCommand, TEvent>
    {
        public bool Equals(Conflict<TCommand, TEvent> other)
        {
            return EqualityComparer<TCommand>.Default.Equals(Command, other.Command) && EqualityComparer<TEvent>.Default.Equals(Event, other.Event) &&
                   string.Equals(Reason, other.Reason);
        }

        public override bool Equals(object obj)
        {
            return obj is Conflict<TCommand, TEvent> other && Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = EqualityComparer<TCommand>.Default.GetHashCode(Command);
                hashCode = (hashCode * 397) ^ EqualityComparer<TEvent>.Default.GetHashCode(Event);
                hashCode = (hashCode * 397) ^ Reason.GetHashCode();
                return hashCode;
            }
        }

        public static bool operator ==(Conflict<TCommand, TEvent> left, Conflict<TCommand, TEvent> right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Conflict<TCommand, TEvent> left, Conflict<TCommand, TEvent> right)
        {
            return !left.Equals(right);
        }

        public Conflict(TCommand command, TEvent @event, string reason)
        {
            Command = command;
            Event = @event;
            Reason = reason;
        }

        public TCommand Command { get; }
        public TEvent Event { get; }
        public string Reason { get; }
    }
}