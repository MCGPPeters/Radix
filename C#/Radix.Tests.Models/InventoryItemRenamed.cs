namespace Radix.Tests.Models
{
    public class InventoryItemRenamed : InventoryItemEvent
    {

        public InventoryItemRenamed(string name, Address address) : base(address)
        {
            Name = name;

        }

        public string Name { get; }
    }
}
