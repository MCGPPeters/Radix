using System;
using System.Threading.Tasks;

namespace Radix
{
    public delegate Task<Result<TEvent[], CommandDecisionError>> Decide<in TState, TCommand, TEvent>(TState state, TransientCommandDescriptor<TCommand> commandDescriptor) where TCommand : IComparable, IComparable<TCommand>, IEquatable<TCommand>;
}
