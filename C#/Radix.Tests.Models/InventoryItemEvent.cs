namespace Radix.Tests.Models
{
    public abstract class InventoryItemEvent : Event
    {
        protected InventoryItemEvent(Address address) : base(address)
        {
        }
    }

}
