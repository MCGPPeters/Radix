namespace Radix.Inventory.Domain.Data.Events;

public record ItemDeactivated : ItemEvent
{
    public ItemDeactivated()
    {

    }

    public ItemDeactivated(string reason) => Reason = reason;


    public string? Reason { get; init; }

    public long Id { get; init; }
}
