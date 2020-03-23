using System;
using System.Threading.Tasks;

namespace Radix
{
    internal interface Agent<TCommand, TEvent> where TCommand : IComparable, IComparable<TCommand>, IEquatable<TCommand>
    {
        DateTimeOffset LastActivity { get; set; }

        Task<Result<TEvent[], Error[]>> Post(CommandDescriptor<TCommand> command);

        void Deactivate();
    }

}
