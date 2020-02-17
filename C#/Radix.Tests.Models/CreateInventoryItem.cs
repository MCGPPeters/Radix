namespace Radix.Tests.Models
{
    public class CreateInventoryItem : InventoryItemCommand
    {

        public CreateInventoryItem(string name)
        {
            Name = name;
        }

        public string Name { get; }
    }
}
