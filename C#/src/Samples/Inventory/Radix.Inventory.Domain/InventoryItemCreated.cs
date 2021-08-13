namespace Radix.Inventory.Domain;

public record InventoryItemCreated : InventoryItemEvent
{
    public InventoryItemCreated()
    {

    }

    public InventoryItemCreated(long Id, string Name, bool Activated, int Count)
    {
        this.Id = Id;
        this.Name = Name;
        this.Activated = Activated;
        this.Count = Count;
    }

    public long Id { get; init; }
    public string? Name { get; init; }
    public int Count { get; init; }
    public bool Activated { get; init; }
}
