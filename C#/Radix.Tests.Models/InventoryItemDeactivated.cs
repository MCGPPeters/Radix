namespace Radix.Tests.Models
{
    public class InventoryItemDeactivated : InventoryItemEvent
    {
        public InventoryItemDeactivated(Address address) : base(address)
        {
        }
    }
}
