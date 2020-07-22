namespace Radix.Blazor.Inventory.Server.Pages
{
    public record CounterIncremented : Event
    {
        public Address? Address { get; init; }
    }
}
