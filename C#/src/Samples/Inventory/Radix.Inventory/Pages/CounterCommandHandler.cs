namespace Radix.Blazor.Inventory.Server.Pages;

public class CounterCommandHandler : CommandHandler<Counter, IncrementCommand, CounterIncremented, CounterCommandHandler>
{
    public static Update<Counter, CounterIncremented> Update => (state, @event) =>
    {
        Counter? newState = state with { Count = state.Count + 1 };
        return newState;
    };

    public static Decide<Counter, IncrementCommand, CounterIncremented> Decide => (state, command) =>
    {
        return Task.FromResult(Ok<CounterIncremented[], CommandDecisionError>(new[] { new CounterIncremented() }));
    };
}
