using System;
using Radix.Validated;
using static Radix.Validated.Extensions;

namespace Radix.Inventory.Domain
{
    public record DeactivateInventoryItem(string Reason) : InventoryItemCommand
    {
        public static Validated<InventoryItemCommand> Create(string reason) => Valid(New)
            .Apply(!string.IsNullOrEmpty(reason) ? Valid(reason) : Invalid<string>("A reason for deactivation must be provided"));

        private static Func<string, InventoryItemCommand> New => (reason)=>
            new DeactivateInventoryItem(reason);
       
    }
}
