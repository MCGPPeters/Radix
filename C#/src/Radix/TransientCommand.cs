namespace Radix;

/// <summary>
///     A transient command is a command of which the resulting events have not been committed to an event stream
/// </summary>
/// <typeparam name="TCommand"></typeparam>
public class TransientCommandDescriptor<TCommand>
{
    public TransientCommandDescriptor(Id recipient, TCommand command)
    {
        Recipient = recipient;
        Command = command;
        MessageId = new MessageId(Guid.NewGuid());
        CorrelationId = MessageId;
    }

    public Id Recipient { get; }

    public TCommand Command { get; }

    public MessageId MessageId { get; }

    /// <summary>
    ///     Since a command is always a the start of a message chain, its message id is the correlation id as well
    /// </summary>
    public MessageId CorrelationId { get; }
}
