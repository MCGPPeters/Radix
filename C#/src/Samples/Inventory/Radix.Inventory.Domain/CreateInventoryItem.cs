using Radix.Validated;

namespace Radix.Inventory.Domain;


public class InventoryItemIdValidator : Validator<long>
{
    public static Func<long, Validated<long>> Validate => id => id > 0 ? Valid(id) : Invalid<long>("Id must be larger than 0");
}


[ConstrainedAlias<long, InventoryItemIdValidator>]
public partial record InventoryItemId
{

}

public record CreateInventoryItem : InventoryItemCommand
{
    private CreateInventoryItem(InventoryItemId Id, string Name, bool Activated, int Count)
    {
        this.Id = Id;
        this.Name = Name;
        this.Activated = Activated;
        this.Count = Count;
    }

    private static Func<InventoryItemId, string, bool, int, InventoryItemCommand> New => (id, name, activated, count) =>
        new CreateInventoryItem(id, name, activated, count);

    public InventoryItemId Id { get; }
    public string Name { get; }
    public bool Activated { get; }
    public int Count { get; }

    public static Validated<InventoryItemCommand> Create(long id, string? name, bool activated, int count) => Valid(New)
        .Apply(InventoryItemId.Create(id))
        .Apply(name.IsNotNullNorEmpty("An inventory item must have a name"))
        .Apply(Valid(activated))
        .Apply(
            count > 0
                ? Valid(count)
                : Invalid<int>("A new inventory item should have at least 1 instance"));
}
