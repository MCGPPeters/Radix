namespace Radix.Inventory.Domain
{
    public record ItemsCheckedInToInventory : InventoryItemEvent
    {
        public ItemsCheckedInToInventory()
        {

        }

        public int Amount { get; init; }
        public long Id { get; init; }
    }
}
