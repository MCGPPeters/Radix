namespace Radix.Tests.Models
{
    public class InventoryItemRenamed : InventoryItemEvent
    {

        public InventoryItemRenamed(string name)
        {
            Name = name;

        }

        public string Name { get; }
    }
}
