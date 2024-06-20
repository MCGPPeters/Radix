using Radix.Data;
using Version = Radix.Domain.Data.Version;

namespace Radix.Domain.Data;

public interface IInstance<TState, in TCommand, TEvent>
{
}

public record Instance<TState, TCommand, TEvent> : IInstance<TState, TCommand, TEvent> where TState : Aggregate<TState, TCommand, TEvent>
{
    public required Aggregate.Address Address { get; init; }
    public required TState State { get; init; }
    public required Version Version { get; init; }

    public required IEnumerable<TEvent> History { get; init; }
    public Func<Validated<TCommand>, Task<Validated<Instance<TState, TCommand, TEvent>>>> Handle { get; set; } = null!;
}
