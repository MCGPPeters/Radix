using System;
using static Radix.Validated.Extensions;

namespace Radix.Inventory.Domain
{
    public record DeactivateInventoryItem(long Id, string Reason) : InventoryItemCommand
    {

        private static Func<long, string, InventoryItemCommand> New => (id, reason) =>
            new DeactivateInventoryItem(id, reason);

        public static Validated<InventoryItemCommand> Create(long id, string reason) => Valid(New)
            .Apply(Valid(id))
            .Apply(!string.IsNullOrEmpty(reason) ? Valid(reason) : Invalid<string>("A reason for deactivation must be provided"));
    }
}
