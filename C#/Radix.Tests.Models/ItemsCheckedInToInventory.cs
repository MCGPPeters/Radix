namespace Radix.Tests.Models
{
    public class ItemsCheckedInToInventory : InventoryItemEvent
    {

        public ItemsCheckedInToInventory(int amount, Address aggregate) : base(aggregate) => Amount = amount;

        public int Amount { get; }

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
