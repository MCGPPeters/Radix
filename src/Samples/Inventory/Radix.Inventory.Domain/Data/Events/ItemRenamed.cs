namespace Radix.Inventory.Domain.Data.Events;

public record ItemRenamed : InventoryEvent
{

    public required long Id { get; init; }
    public required string Name { get; init; }
}
