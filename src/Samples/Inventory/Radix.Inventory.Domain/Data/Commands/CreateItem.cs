using Radix.Data;
using Radix.Data.String.Validity;

namespace Radix.Inventory.Domain.Data.Commands;

public record CreateItem : ItemCommand
{
    private CreateItem(Id Id, string Name, bool Activated, int Count)
    {
        this.Id = Id;
        this.Name = Name;
        this.Activated = Activated;
        this.Count = Count;
    }

    private static Func<Id, string, bool, int, ItemCommand> New => (id, name, activated, count) =>
        new CreateItem(id, name, activated, count);

    public Id Id { get; }
    public string Name { get; }
    public bool Activated { get; }
    public int Count { get; }

    public static Validated<ItemCommand> Create(long id, string? name, bool activated, int count) => Valid(New)
        .Apply(Id.Create(id))
        .Apply(IsNotNullOrEmpty.Validate("Name", name))
        .Apply(Valid(activated))
        .Apply(
            count > 0
                ? Valid(count)
                : Invalid<int>("A new inventory item should have at least 1 instance"));
}
