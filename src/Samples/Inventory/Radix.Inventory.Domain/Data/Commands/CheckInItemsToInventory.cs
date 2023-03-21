﻿using Radix.Data;

namespace Radix.Inventory.Domain.Data.Commands;

public record CheckInItemsToInventory(long Id, int Amount) : InventoryCommand
{
    private static Func<long, int, InventoryCommand> New => (id, amount) =>
        new CheckInItemsToInventory(id, amount);

    public static Validated<InventoryCommand> Create(long id, int amount) =>
        Valid(New)
            .Apply(
                id > 0
                    ? Valid(id)
                    : Invalid<long>("Inventory item id", $"The id of the inventory item must be greater than 0 but is '{id}'"))
            .Apply(
                amount > 0
                    ? Valid(amount)
                    : Invalid<int>("Amount", $"The amount of the inventory item must be greater than 0 but is '{amount}'"));
}
