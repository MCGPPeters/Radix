namespace Radix;

public record MessageId(Guid Value) : Alias<MessageId, Guid>
{
    public static implicit operator MessageId(Guid guid) => new(guid);
    public static implicit operator Guid(MessageId messageId) => messageId;
}
