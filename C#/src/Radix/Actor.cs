using System;
using System.Threading.Tasks;

namespace Radix
{
    internal interface Actor<TCommand, TEvent>
    {
        DateTimeOffset LastActivity { get; set; }

        Task<Result<TEvent[], Error[]>> Post(TransientCommandDescriptor<TCommand> transientCommand);

        void Deactivate();
    }

}
