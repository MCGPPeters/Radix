namespace Radix.Inventory.Domain.Data.Commands;

public abstract record ItemCommand : Command<ItemCommand>, InventoryCommand
{
}

public interface InventoryCommand
{
}
