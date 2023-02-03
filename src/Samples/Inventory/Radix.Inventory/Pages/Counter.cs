using Radix.Domain.Data;

namespace Radix.Inventory.Pages;

public record Counter: Aggregate<Counter, IncrementCommand, CounterIncremented>
{
    public int Count { get; init; }

    public static Counter Create() => new();

    public static Counter Apply(Counter state, CounterIncremented @event) => new Counter {Count = state.Count + 1};

    public static IEnumerable<CounterIncremented> Decide(Counter state, IncrementCommand command) => new[]{new CounterIncremented()};

    public static IAsyncEnumerable<CounterIncremented> ResolveConflicts(Counter state, IEnumerable<CounterIncremented> ourEvents, IOrderedAsyncEnumerable<Event<CounterIncremented>> theirEvents) => ourEvents.ToAsyncEnumerable();
}
