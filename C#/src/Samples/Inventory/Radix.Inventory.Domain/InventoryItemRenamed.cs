namespace Radix.Inventory.Domain;

public record InventoryItemRenamed : InventoryItemEvent
{

    public long Id { get; init; }
    public string? Name { get; init; }
}
