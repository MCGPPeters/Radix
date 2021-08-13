using Radix.Result;

namespace Radix.Blazor.Inventory.Server.Pages;

public record Counter
{
    public static readonly Update<Counter, CounterIncremented> Update = (state, @event) =>
    {
        Counter? newState = state with { Count = state.Count + 1 };
        return newState;
    };

    public static readonly Decide<Counter, IncrementCommand, CounterIncremented> Decide = (state, command) =>
    {
        return Task.FromResult(Extensions.Ok<CounterIncremented[], CommandDecisionError>(new[] { new CounterIncremented() }));
    };

    public int Count { get; init; }
}
