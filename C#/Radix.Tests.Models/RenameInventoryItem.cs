namespace Radix.Tests.Models
{
    public class RenameInventoryItem : InventoryItemCommand
    {

        public RenameInventoryItem(string name)
        {
            Name = name;
        }

        public string Name { get; }
    }
}
