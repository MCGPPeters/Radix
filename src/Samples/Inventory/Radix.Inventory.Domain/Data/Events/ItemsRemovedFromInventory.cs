namespace Radix.Inventory.Domain.Data.Events;

public record ItemsRemovedFromInventory : ItemEvent
{
    public ItemsRemovedFromInventory()
    {

    }


    public ItemsRemovedFromInventory(int Amount, long Id)
    {
        this.Amount = Amount;
        this.Id = Id;
    }

    public int Amount { get; init; }
    public long Id { get; init; }
}
