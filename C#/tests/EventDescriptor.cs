using System.Collections.Generic;

namespace Radix.Tests
{
    /// <summary>
    ///     Combining the metadata of an event with the even itself
    /// </summary>
    public struct EventDescriptor<TEvent>
    {
        public bool Equals(EventDescriptor<TEvent> other)
        {
            return EqualityComparer<TEvent>.Default.Equals(Event, other.Event) && Version.Equals(other.Version);
        }

        public override bool Equals(object obj)
        {
            return obj is EventDescriptor<TEvent> other && Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (EqualityComparer<TEvent>.Default.GetHashCode(Event) * 397) ^ Version.GetHashCode();
            }
        }

        public EventDescriptor(TEvent @event, Version version)
        {
            Event = @event;
            Version = version;
        }

        public TEvent Event { get; }

        public Version Version { get; }

        public static bool operator ==(EventDescriptor<TEvent> left, EventDescriptor<TEvent> right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(EventDescriptor<TEvent> left, EventDescriptor<TEvent> right)
        {
            return !(left == right);
        }
    }
}