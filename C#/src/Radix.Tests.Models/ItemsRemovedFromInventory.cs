namespace Radix.Tests.Models
{
    public class ItemsRemovedFromInventory : InventoryItemEvent
    {
        public ItemsRemovedFromInventory(int amount) => Amount = amount;

        public int Amount { get; }

        protected bool Equals(ItemsRemovedFromInventory other) => Amount == other.Amount;

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

            return obj.GetType() == GetType() && Equals((ItemsRemovedFromInventory)obj);
        }

        public override int GetHashCode() => Amount;

        public long Id { get; set; }
    }
}
