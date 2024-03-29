﻿using Radix.Data;

namespace Radix.Inventory.Domain.Data.Commands;

public record RemoveItemsFromInventory : InventoryCommand
{
    public long Id { get; }
    public int Amount { get; }

    private RemoveItemsFromInventory(long id, int amount)

    {
        Id = id;
        Amount = amount;
    }

    private static Func<long, int, InventoryCommand> New => (id, amount) =>
        new RemoveItemsFromInventory(id, amount);


    public static Validated<InventoryCommand> Create(long id, int amount) => Valid(New)
        .Apply(
            id > 0
                ? Valid(id)
                : Invalid<long>("Address", ""))
        .Apply(
            amount > 0
                ? Valid(amount)
                : Invalid<int>("Amount", ""));
}
