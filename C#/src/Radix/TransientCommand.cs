using System;

namespace Radix
{
    /// <summary>
    ///     A transient command is a command of which the resulting events have not been committed to an event stream
    /// </summary>
    /// <typeparam name="TCommand"></typeparam>
    public class TransientCommandDescriptor<TCommand> where TCommand : IComparable, IComparable<TCommand>, IEquatable<TCommand>
    {
        public TransientCommandDescriptor(Address recipient, TCommand command)
        {
            Recipient = recipient;
            Command = command;
            CorrelationId = MessageId;
            MessageId = new MessageId(Guid.NewGuid());
        }

        public Address Recipient { get; }

        public TCommand Command { get; }

        public MessageId MessageId { get; }

        /// <summary>
        ///     Since a command is always a the start of a message chain, its message id is the correlation id as well
        /// </summary>
        public MessageId CorrelationId { get; }
    }
}
