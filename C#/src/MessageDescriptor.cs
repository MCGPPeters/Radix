namespace Radix
{
    public interface MessageDescriptor
    {
        public MessageId MessageId { get; }
        public MessageId CorrelationId { get; }
    }
}
