using System;
using static Radix.Validated.Extensions;

namespace Radix.Inventory.Domain
{
    public class CheckInItemsToInventory : InventoryItemCommand
    {

        public CheckInItemsToInventory(long id,  int amount)
        {
            Id = id;
            Amount = amount;
        }

        public int Amount { get; }


        private static Func<long, int, InventoryItemCommand> New => (id, amount) =>
            new CheckInItemsToInventory(id, amount);

        public long Id { get; internal set; }

        public int CompareTo(object obj) => throw new NotImplementedException();

        public int CompareTo(InventoryItemCommand other) => throw new NotImplementedException();

        public bool Equals(InventoryItemCommand other) => throw new NotImplementedException();

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
