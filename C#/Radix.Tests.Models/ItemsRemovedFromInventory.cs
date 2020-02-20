namespace Radix.Tests.Models
{
    public class ItemsRemovedFromInventory : InventoryItemEvent
    {
        public ItemsRemovedFromInventory(int amount)
        {
            Amount = amount;
        }

        public int Amount { get; }
    }
}
