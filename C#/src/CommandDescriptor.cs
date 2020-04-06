using System;
using Radix.Validated;

namespace Radix
{
    public class CommandDescriptor<TCommand> where TCommand : IComparable, IComparable<TCommand>, IEquatable<TCommand>
    {
        public CommandDescriptor(Address address, TCommand command, IVersion expectedVersion)
        {
            Address = address;
            Command = command;
            ExpectedVersion = expectedVersion ?? throw new ArgumentNullException(nameof(expectedVersion));
        }

        public Address Address { get; }

        public Command<TCommand> Command { get; }

        public IVersion ExpectedVersion { get; }
    }

}
