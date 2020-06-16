using System;

namespace Radix.Inventory.Domain
{
    public interface InventoryItemCommand : IComparable, IComparable<InventoryItemCommand>, IEquatable<InventoryItemCommand>
    {
    }
}
