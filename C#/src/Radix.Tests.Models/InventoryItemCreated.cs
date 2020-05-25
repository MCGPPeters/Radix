namespace Radix.Tests.Models
{
    public class InventoryItemCreated : InventoryItemEvent
    {

        public InventoryItemCreated(string name, bool activated, int count, Address aggregate) : base(aggregate)
        {
            Name = name;
            Activated = activated;
            Count = count;
        }

        public string Name { get; }
        public bool Activated { get; }
        public int Count { get; set; }

        protected bool Equals(InventoryItemCreated other) => string.Equals(Name, other.Name);

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            return obj.GetType() == GetType() && Equals((InventoryItemCreated)obj);
        }

        public override int GetHashCode() => Name != null ? Name.GetHashCode() : 0;
    }
}
