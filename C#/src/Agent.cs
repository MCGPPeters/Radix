using System;

namespace Radix
{
    internal interface Agent<TCommand>
    {
        void Post(CommandDescriptor<TCommand> command);
        DateTimeOffset LastActivity { get; set; }

        void Deactivate();
    }
}