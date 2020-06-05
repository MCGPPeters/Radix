using System;

namespace Radix.Tests.Models
{
    public class InventoryItemRenamed : InventoryItemEvent
    {

        public string Name { get; set; }
        public Address? Address { get; set; }

        public bool Equals(InventoryItemRenamed? other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return Address != null && (Name == other.Name && Address.Equals(other.Address));
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            return obj.GetType() == GetType() && Equals((InventoryItemRenamed)obj);

        }

        public override int GetHashCode() => HashCode.Combine(Name, Address);

        public static bool operator ==(InventoryItemRenamed? left, InventoryItemRenamed? right) => Equals(left, right);

        public static bool operator !=(InventoryItemRenamed? left, InventoryItemRenamed? right) => !Equals(left, right);
    }
}
