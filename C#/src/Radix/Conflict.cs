using System.Collections.Generic;

namespace Radix
{
    public class Conflict<TCommand, TEvent> where TCommand : notnull where  TEvent : notnull
    {

        public Conflict(TCommand command, TEvent @event, string reason)
        {
            Command = command;
            Event = @event;
            Reason = reason;
        }

        private TCommand Command { get; }
        private TEvent Event { get; }
        public string Reason { get; }

        public bool Equals(Conflict<TCommand, TEvent> other) => EqualityComparer<TCommand>.Default.Equals(Command, other.Command) &&
                                                                EqualityComparer<TEvent>.Default.Equals(Event, other.Event) &&
                                                                string.Equals(Reason, other.Reason);

        public override bool Equals(object? obj) => obj is Conflict<TCommand, TEvent> other && Equals(other);

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = EqualityComparer<TCommand>.Default.GetHashCode(Command);
                hashCode = (hashCode * 397) ^ EqualityComparer<TEvent>.Default.GetHashCode(Event);
                hashCode = (hashCode * 397) ^ Reason.GetHashCode();
                return hashCode;
            }
        }

        public static bool operator ==(Conflict<TCommand, TEvent> left, Conflict<TCommand, TEvent> right) => left.Equals(right);

        public static bool operator !=(Conflict<TCommand, TEvent> left, Conflict<TCommand, TEvent> right) => !left.Equals(right);
    }
}
