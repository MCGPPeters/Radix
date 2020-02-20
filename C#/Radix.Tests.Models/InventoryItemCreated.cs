namespace Radix.Tests.Models
{
    public class InventoryItemCreated : InventoryItemEvent
    {

        public InventoryItemCreated(string name)
        {
            Name = name;

        }

        public string Name { get; }

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
