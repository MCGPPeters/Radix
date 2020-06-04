namespace Radix
{
    public delegate TransientEventDescriptor<TFormat> ToTransientEventDescriptor<TEvent, TFormat>(MessageId messageId, TEvent @event, Serialize<TEvent, TFormat> serialize,
        EventMetaData eventMetaData, Serialize<EventMetaData, TFormat> serializeMetaData);
}
