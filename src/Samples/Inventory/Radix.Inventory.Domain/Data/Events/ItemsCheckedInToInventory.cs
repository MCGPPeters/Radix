namespace Radix.Inventory.Domain.Data.Events;

public record ItemsCheckedInToInventory : ItemEvent
{

    public int Amount { get; init; }
    public long Id { get; init; }
}
