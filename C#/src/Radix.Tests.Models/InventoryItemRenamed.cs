namespace Radix.Tests.Models
{
    public class InventoryItemRenamed : InventoryItemEvent
    {

        public InventoryItemRenamed(string name, Address aggregate) : base(aggregate) => Name = name;

        public string Name { get; }
    }
}
