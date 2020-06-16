using System;
using Radix.Validated;
using static Radix.Validated.Extensions;

namespace Radix.Inventory.Domain
{
    public class DeactivateInventoryItem : InventoryItemCommand
    {
        public string Reason { get; }

        private DeactivateInventoryItem(string reason)
        {
            Reason = reason;

        }

        public static Validated<InventoryItemCommand> Create(string reason) => Valid(New)
            .Apply(!string.IsNullOrEmpty(reason) ? Valid(reason) : Invalid<string>("A reason for deactivation must be provided"));

        private static Func<string, InventoryItemCommand> New => (reason)=>
            new DeactivateInventoryItem(reason);

        public int CompareTo(object obj) => throw new NotImplementedException();

        public int CompareTo(InventoryItemCommand other) => throw new NotImplementedException();

        public bool Equals(InventoryItemCommand other) => throw new NotImplementedException();

        
    }
}
