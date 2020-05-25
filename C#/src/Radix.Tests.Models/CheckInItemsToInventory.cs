using System;
using static Radix.Validated.Extensions;

namespace Radix.Tests.Models
{
    public class CheckInItemsToInventory : InventoryItemCommand
    {

        public CheckInItemsToInventory(int amount) => Amount = amount;

        public int Amount { get; }


        private static Func<int, InventoryItemCommand> New => amount =>
            new CheckInItemsToInventory(amount);

        public int CompareTo(object obj) => throw new NotImplementedException();

        public int CompareTo(InventoryItemCommand other) => throw new NotImplementedException();

        public bool Equals(InventoryItemCommand other) => throw new NotImplementedException();

        public static Validated<InventoryItemCommand> Create(int amount) => Valid(New)
            .Apply(
                amount > 0
                    ? Valid(amount)
                    : Invalid<int>(""));
    }
}
