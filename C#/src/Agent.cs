using System;

namespace Radix
{
    internal interface Agent<TCommand>
    {
        DateTimeOffset LastActivity { get; set; }

        void Post(CommandDescriptor<TCommand> command);

        void Deactivate();
    }
}
