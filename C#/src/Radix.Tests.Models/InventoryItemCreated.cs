namespace Radix.Tests.Models
{
    public class InventoryItemCreated : InventoryItemEvent
    {
        public string Name { get; set; }
        public bool Activated { get; set; }
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

        public Address? Address { get; set; }
    }
}
