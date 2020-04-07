using System;

namespace Radix.Tests.Models
{
    public interface InventoryItemCommand : IComparable, IComparable<InventoryItemCommand>, IEquatable<InventoryItemCommand>
    {
    }
}
