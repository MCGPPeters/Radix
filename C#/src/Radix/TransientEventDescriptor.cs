using System;

namespace Radix
{
    /// <summary>
    ///     A descriptor for an event that has not yet been persisted into an event stream. Combining the metadata with the
    ///     event itself
    /// </summary>
    /// <typeparam name="TEvent">The type of the event</typeparam>
    public class TransientEventDescriptor<TEvent> : MessageDescriptor
    {

        /// <summary>
        /// </summary>
        /// <param name="causingMessageDescriptor">The description of the message that caused this event to be created</param>
        /// <param name="event"></param>
        public TransientEventDescriptor(MessageDescriptor causingMessageDescriptor, TEvent @event)
        {
            Event = @event;
            CausationId = causingMessageDescriptor.MessageId;
            CorrelationId = causingMessageDescriptor.CorrelationId;
        }

        public TEvent Event { get; }
        public MessageId CausationId { get; }

        public MessageId MessageId { get; } = new MessageId(Guid.NewGuid());
        public MessageId CorrelationId { get; }
    }
}
