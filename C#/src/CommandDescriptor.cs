using System;

namespace Radix
{
    public class CommandDescriptor<TCommand> where TCommand : IComparable, IComparable<TCommand>, IEquatable<TCommand>
    {
        public CommandDescriptor(Address address, TCommand command)
        {
            Address = address;
            Command = command;
        }

        public Address Address { get; }

        public Command<TCommand> Command { get; }
    }

}
