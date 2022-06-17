namespace Radix.Inventory.Domain.Data.Events;

public record ItemRenamed : ItemEvent
{

    public long Id { get; init; }
    public string? Name { get; init; }
}
