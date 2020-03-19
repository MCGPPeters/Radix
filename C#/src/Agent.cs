using System;
using System.Threading.Tasks;

namespace Radix
{
    internal interface Agent<TCommand, TEvent>
    {
        DateTimeOffset LastActivity { get; set; }

        Task<Result<TEvent[], string[]>> Post(CommandDescriptor<TCommand> command);

        void Deactivate();
    }
}
