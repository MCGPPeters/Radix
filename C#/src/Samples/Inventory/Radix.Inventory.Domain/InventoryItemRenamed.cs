using System;

namespace Radix.Inventory.Domain
{
    public record InventoryItemRenamed(string Name) : InventoryItemEvent
    {
        public long Id { get; set; }
    }
}
