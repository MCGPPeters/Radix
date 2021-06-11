using System;
using System.Threading.Tasks;

namespace Radix
{
    internal interface Actor<TCommand, TEvent> : IDisposable
    {
        DateTimeOffset LastActivity { get; set; }

        Task<Result<TEvent[], Error[]>> Post(TransientCommandDescriptor<TCommand> transientCommand);

        void Stop();

        Task Start();
    }

}
