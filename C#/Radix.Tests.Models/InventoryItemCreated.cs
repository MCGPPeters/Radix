namespace Radix.Tests.Models
{
    public class InventoryItemCreated : InventoryItemEvent
    {

        public InventoryItemCreated(string name, bool activated, int count, Address address) : base(address)
        {
            Name = name;
            Activated = activated;
            Count = count;
        }

        public string Name { get; }
        public bool Activated { get; }
        public int Count { get; set; }

        protected bool Equals(InventoryItemCreated other)
        {
            return string.Equals(Name, other.Name);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((InventoryItemCreated) obj);
        }

        public override int GetHashCode()
        {
            return Name != null ? Name.GetHashCode() : 0;
        }
    }
}
