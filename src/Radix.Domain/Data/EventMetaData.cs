namespace Radix.Domain.Data;

public class EventMetaData
{
    public EventMetaData(MessageId causationId, MessageId correlationId)
    {
        CausationId = causationId;
        CorrelationId = correlationId;
    }

    public MessageId CausationId { get; }
    public MessageId MessageId { get; } = (MessageId)Guid.NewGuid();
    public MessageId CorrelationId { get; }
}
