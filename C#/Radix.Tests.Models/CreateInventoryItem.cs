namespace Radix.Tests.Models
{
    public class CreateInventoryItem : InventoryItemCommand
    {

        public CreateInventoryItem(string name, bool activated, int count)
        {
            Name = name;
            Activated = activated;
            Count = count;
        }

        public string Name { get; }
        public bool Activated { get; }
        public int Count { get; }
    }
}
