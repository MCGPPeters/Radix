using System.Collections.Generic;

namespace Radix
{
    /// <summary>
    ///     Combining the metadata of a persisted event with the event itself
    /// </summary>
    public class EventDescriptor<TEvent> : MessageDescriptor
    {

        public EventDescriptor(Address aggregate, MessageId messageId, MessageId causationId, MessageId correlationId, TEvent @event, Version version)
        {
            Event = @event;
            Aggregate = aggregate;
            CausationId = causationId;
            CorrelationId = correlationId;
            MessageId = messageId;
            Version = version;
        }

        public TEvent Event { get; }
        public Address Aggregate { get; }

        public Version Version { get; }
        public MessageId CausationId { get; }

        public MessageId MessageId { get; }
        public MessageId CorrelationId { get; }

        public bool Equals(EventDescriptor<TEvent> other) => EqualityComparer<TEvent>.Default.Equals(Event, other.Event) && Version.Equals(other.Version);

        public override bool Equals(object obj) => obj is EventDescriptor<TEvent> other && Equals(other);

        public override int GetHashCode()
        {
            unchecked
            {
                return (EqualityComparer<TEvent>.Default.GetHashCode(Event) * 397) ^ Version.GetHashCode();
            }
        }

        public static bool operator ==(EventDescriptor<TEvent> left, EventDescriptor<TEvent> right) => left.Equals(right);

        public static bool operator !=(EventDescriptor<TEvent> left, EventDescriptor<TEvent> right) => !(left == right);
    }

}
