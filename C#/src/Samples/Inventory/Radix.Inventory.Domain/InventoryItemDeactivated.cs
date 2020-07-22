namespace Radix.Inventory.Domain
{
    public record InventoryItemDeactivated : InventoryItemEvent
    {
        public InventoryItemDeactivated()
        {

        }

        public InventoryItemDeactivated(string Reason)
        {
            this.Reason = Reason;
        }

        public string? Reason { get; init; }
    }
}
