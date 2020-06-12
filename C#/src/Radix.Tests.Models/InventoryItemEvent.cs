namespace Radix.Tests.Models
{
    public abstract class InventoryItemEvent : Event
    {
        public InventoryItemEvent()
        {
            
        }
        public Address Address { get; set; }
    }

}
