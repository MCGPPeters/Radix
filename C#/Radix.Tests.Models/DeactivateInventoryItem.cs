using System;

namespace Radix.Tests.Models
{
    public struct DeactivateInventoryItem : InventoryItemCommand
    {
        public int CompareTo(object obj)
        {
            throw new NotImplementedException();
        }

        public int CompareTo(InventoryItemCommand other)
        {
            throw new NotImplementedException();
        }

        public bool Equals(InventoryItemCommand other)
        {
            throw new NotImplementedException();
        }
    }
}
