using System;

namespace Radix
{
    public class EventMetaData
    {
        public EventMetaData(MessageId causationId, MessageId correlationId)
        {
            CausationId = causationId;
            CorrelationId = correlationId;
        }

        public MessageId CausationId { get; }
        public MessageId MessageId { get; } = new MessageId(Guid.NewGuid());
        public MessageId CorrelationId { get; }
    }
}
