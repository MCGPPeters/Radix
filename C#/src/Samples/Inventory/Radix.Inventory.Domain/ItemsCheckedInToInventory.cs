namespace Radix.Inventory.Domain
{
    public record ItemsCheckedInToInventory(int Amount, long Id) : InventoryItemEvent;
}
