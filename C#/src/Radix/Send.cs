using System;
using System.Threading.Tasks;

namespace Radix
{
    public delegate Task<Result<TEvent[], Error[]>> Send<TCommand, TEvent>(Validated<TCommand> validatedCommand)
        where TCommand : IComparable, IComparable<TCommand>, IEquatable<TCommand>;
}
