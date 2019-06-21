namespace Radix.Tests
{
    public class CommandDescriptor<TCommand>
    {
        public CommandDescriptor(Address address, TCommand command, IVersion expectedVersion)
        {
            Address = address;
            Command = command;
            ExpectedVersion = expectedVersion;
        }

        public Address Address { get; }

        public TCommand Command { get; }

        public IVersion ExpectedVersion { get; }
    }
}