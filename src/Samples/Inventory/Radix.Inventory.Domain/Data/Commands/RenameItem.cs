namespace Radix.Inventory.Domain.Data.Commands;

public record RenameItem(long Id, string Name) : ItemCommand;
