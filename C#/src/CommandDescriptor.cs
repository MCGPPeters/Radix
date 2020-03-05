using System;

namespace Radix
{
    public class CommandDescriptor<TCommand>
    {
        public CommandDescriptor(Address address, TCommand command, IVersion expectedVersion)
        {
            Address = address;
            Command = command ?? throw new ArgumentNullException(nameof(command));
            ExpectedVersion = expectedVersion ?? throw new ArgumentNullException(nameof(expectedVersion));
        }

        public Address Address { get; }

        public TCommand Command { get; }

        public IVersion ExpectedVersion { get; }
    }
}
