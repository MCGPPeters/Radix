namespace Radix.Inventory.Domain;

public record InventoryItem
{
    public InventoryItem()
    {
        Name = "";
        Activated = true;
        Count = 0;
        ReasonForDeactivation = "";
    }

    public InventoryItem(string name, bool activated, int count)
    {
        Name = name;
        Activated = activated;
        Count = count;
        ReasonForDeactivation = "";
    }

    public string? ReasonForDeactivation { get; init; }
    public string? Name { get; init; }
    public bool Activated { get; init; }
    public int Count { get; init; }
}
