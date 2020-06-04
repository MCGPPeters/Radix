using System;
using System.Text.Json;

namespace Radix
{

    public delegate TFormat Serialize<in T, out TFormat>(T input);
    public delegate T Parse<out T, in TFormat>(TFormat input);

    /// <summary>
    ///     A descriptor for an event that has not yet been persisted into an event stream. Combining the metadata with the
    ///     event itself
    /// </summary>
    public class TransientEventDescriptor<TFormat>
    {
        /// <summary>
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="serialize"></param>
        /// <param name="event"></param>
        /// <param name="eventMetaData"></param>
        /// <param name="causationId"></param>
        /// <param name="correlationId"></param>
        /// <param name="messageId"></param>
        public TransientEventDescriptor(EventType eventType, TFormat @event, TFormat eventMetaData, MessageId messageId)
        {
            Event = @event;
            EventMetaData = eventMetaData;
            MessageId = messageId;
            EventType = eventType;
        }

        public TFormat Event { get; }

        public EventType EventType { get; }

        public TFormat EventMetaData { get; }
        public MessageId MessageId { get;  }
    }
}
