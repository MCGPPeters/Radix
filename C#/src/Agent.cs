using System;
using System.Threading.Tasks;

namespace Radix
{
    internal interface Agent<TCommand>
    {
        DateTimeOffset LastActivity { get; set; }

        Task Post(CommandDescriptor<TCommand> command);

        void Deactivate();
    }
}
