namespace Radix.Tests.Models
{
    public abstract class InventoryItemEvent : Event
    {

        public Address Address { get; set; }
    }

}
