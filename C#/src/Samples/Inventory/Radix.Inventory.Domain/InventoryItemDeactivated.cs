namespace Radix.Inventory.Domain
{
    public record InventoryItemDeactivated : InventoryItemEvent
    {
        public InventoryItemDeactivated()
        {

        }

        public InventoryItemDeactivated(long id, string reason)
        {
            Id = id;
            Reason = reason;
        }


        public string? Reason { get; init; }

        public long Id { get; init; }
    }
}
