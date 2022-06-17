namespace Radix.Inventory.Domain.Data;

public record Item
{
    public Item()
    {
        Name = "";
        Activated = true;
        Count = 0;
        ReasonForDeactivation = "";
    }

    public Item(string name, bool activated, int count)
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
