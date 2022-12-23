using Radix.Data;

namespace Radix.Inventory.Domain.Data.Commands;

public record RemoveItemsFromInventory(long Id, int Amount) : ItemCommand
{

    private static Func<long, int, ItemCommand> New => (id, amount) =>
        new RemoveItemsFromInventory(id, amount);


    public static Validated<ItemCommand> Create(long id, int amount) => Valid(New)
        .Apply(
            id > 0
                ? Valid(id)
                : Invalid<long>("Id", ""))
        .Apply(
            amount > 0
                ? Valid(amount)
                : Invalid<int>("Amount", ""));
}
