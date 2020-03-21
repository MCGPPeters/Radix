using System;
using static Radix.Validated.Extensions;

namespace Radix.Tests.Models
{
    public class RemoveItemsFromInventory : InventoryItemCommand
    {

        private RemoveItemsFromInventory(int amount)
        {
            Amount = amount;
        }

        public int Amount { get; }


        private static Func<int, RemoveItemsFromInventory> New => (amount) =>
            new RemoveItemsFromInventory(amount);

        public static Validated<InventoryItemCommand> Create(int amount)
        {
            return Valid(New)
                .Apply(amount > 0
                    ? Valid(amount)
                    : Invalid<int>(""));
        }

        public int CompareTo(object obj)
        {
            throw new NotImplementedException();
        }

        public int CompareTo(InventoryItemCommand other)
        {
            throw new NotImplementedException();
        }

        public bool Equals(InventoryItemCommand other)
        {
            throw new NotImplementedException();
        }
    }
  
}
