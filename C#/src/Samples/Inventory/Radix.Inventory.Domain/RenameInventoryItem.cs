namespace Radix.Inventory.Domain
{
    public record RenameInventoryItem(long Id, string Name) : InventoryItemCommand;

}
