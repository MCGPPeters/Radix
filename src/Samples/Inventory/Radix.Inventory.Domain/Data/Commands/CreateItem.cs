using Radix.Data;
using Radix.Data.String.Validity;

namespace Radix.Inventory.Domain.Data.Commands;

public record CreateItem : InventoryCommand
{
    private CreateItem(Id id, string name, bool activated, int count)
    {
        Id = id;
        Name = name;
        Activated = activated;
        Count = count;
    }

    public Id Id { get; }
    public string Name { get; }
    public bool Activated { get; }
    public int Count { get; }

    public static Validated<InventoryCommand> Create(long id, string name, bool activated, int count)
    {
        var create = (Id i, string __, bool ___, int ____) => (InventoryCommand)new CreateItem(i, name, activated, count);

        return Valid(create)
            .Apply(Id.Create(id))
            .Apply(IsNotNullOrEmpty.Validate("Name")(name))
            .Apply(Valid(activated))
            .Apply(
                count > 0
                    ? Valid(count)
                    : Invalid<int>("Count", "A new inventory item should have at least 1 instance"));
    }
}
