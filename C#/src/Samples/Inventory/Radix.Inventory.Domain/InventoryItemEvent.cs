namespace Radix.Inventory.Domain
{
    public abstract record InventoryItemEvent : Event
    {
        public Address? Address { get; init; }
    }

    public record ItemsRemovedFromInventory(int Amount, long Id) : InventoryItemEvent;
}
