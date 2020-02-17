namespace Radix.Tests.Models
{
    public class CheckInItemsToInventory : InventoryItemCommand
    {

        public CheckInItemsToInventory(int amount)
        {
            Amount = amount;
        }

        public int Amount { get; }
    }
}
