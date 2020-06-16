namespace Radix.Inventory.Domain
{
    public class InventoryItemDeactivated : InventoryItemEvent
    {

        public Address Address { get; set; }
        public string Reason { get; set; }
    }
}
