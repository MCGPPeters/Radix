namespace Radix.Inventory.Domain;

public record InventoryItemDeactivated : InventoryItemEvent
{
    public InventoryItemDeactivated()
    {

    }

    public InventoryItemDeactivated(string reason) => Reason = reason;


    public string? Reason { get; init; }

    public long Id { get; init; }
}
