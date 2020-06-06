namespace Radix.Tests.Models
{
    public class ItemsCheckedInToInventory : InventoryItemEvent
    {

        public int Amount { get; set; }

        public long Id { get; set; }
        public Address Address { get; set; }

        protected bool Equals(ItemsCheckedInToInventory other) => Amount == other.Amount;

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

            return obj.GetType() == GetType() && Equals((ItemsCheckedInToInventory)obj);
        }

        public override int GetHashCode() => Amount;
    }
}
