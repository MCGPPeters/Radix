namespace Radix.Tests.Models
{
    public class ItemsRemovedFromInventory : InventoryItemEvent
    {
        public ItemsRemovedFromInventory(int amount, Address address) : base(address)
        {
            Amount = amount;
        }

        public int Amount { get; }

        protected bool Equals(ItemsRemovedFromInventory other)
        {
            return Amount == other.Amount;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((ItemsRemovedFromInventory)obj);
        }

        public override int GetHashCode()
        {
            return Amount;
        }
    }
}
