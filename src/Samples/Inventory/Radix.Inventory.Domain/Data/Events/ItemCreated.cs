namespace Radix.Inventory.Domain.Data.Events;

public record ItemCreated : ItemEvent
{
    public ItemCreated()
    {

    }

    public ItemCreated(long Id, string Name, bool Activated, int Count)
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
