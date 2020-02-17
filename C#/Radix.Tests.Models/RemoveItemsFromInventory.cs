namespace Radix.Tests.Models
{
    public class RemoveItemsFromInventory : InventoryItemCommand
    {

        public RemoveItemsFromInventory(int amount)
        {
            Amount = amount;
        }

        public int Amount { get; }
    }
}
