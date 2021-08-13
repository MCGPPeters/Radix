using Radix.Validated;
using static Radix.Validated.Extensions;

namespace Radix.Inventory.Domain;

public record CreateInventoryItem(long Id, string Name, bool Activated, int Count) : InventoryItemCommand
{
    private static Func<long, string, bool, int, InventoryItemCommand> New => (id, name, activated, count) =>
        new CreateInventoryItem(id, name, activated, count);

    public static Validated<InventoryItemCommand> Create(long id, string? name, bool activated, int count) => Valid(New)
        .Apply(id > 0 ? Valid(id) : Invalid<long>("Id must be larger than 0"))
        .Apply(name.IsNotNullNorEmpty("An inventory item must have a name"))
        .Apply(Valid(activated))
        .Apply(
            count > 0
                ? Valid(count)
                : Invalid<int>("A new inventory item should have at least 1 instance"));
}
