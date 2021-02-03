using System;
using static Radix.Validated.Extensions;

namespace Radix.Inventory.Domain
{
    public record RemoveItemsFromInventory(long Id, int Amount) : InventoryItemCommand
    {

        private static Func<long, int, InventoryItemCommand> New => (id, amount) =>
            new RemoveItemsFromInventory(id, amount);


        public static Validated<InventoryItemCommand> Create(long id, int amount) => Valid(New)
            .Apply(
                id > 0
                    ? Valid(id)
                    : Invalid<long>(""))
            .Apply(
                amount > 0
                    ? Valid(amount)
                    : Invalid<int>(""));
    }
}
