namespace Radix.Domain.Data;

/// <summary>
///     A transient command is a command of which the resulting events have not been committed to an event stream
/// </summary>
/// <typeparam name="TCommand"></typeparam>
public class TransientCommandDescriptor<TCommand>
{
    public TransientCommandDescriptor(Aggregate.Id recipient, TCommand command)
    {
        Recipient = recipient;
        Command = command;
        MessageId = (MessageId)Guid.NewGuid();
        CorrelationId = MessageId;
    }

    public Aggregate.Id Recipient { get; }

    public TCommand Command { get; }

    public MessageId MessageId { get; }

    /// <summary>
    ///     Since a command is always a the start of a message chain, its message Id is the correlation Id as well
    /// </summary>
    public MessageId CorrelationId { get; }
}
