namespace Radix.Inventory.Domain
{
    public abstract class InventoryItemEvent : Event
    {

        public Address Address { get; set; }
    }

}
