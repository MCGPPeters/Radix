namespace Radix.Inventory.Domain
{
    public record InventoryItemDeactivated(string Reason) : InventoryItemEvent;
}
