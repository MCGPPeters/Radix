namespace Radix.Tests.Models
{
    public class InventoryItemDeactivated : InventoryItemEvent
    {
        public InventoryItemDeactivated(Address aggregate) : base(aggregate)
        {
        }
    }
}
