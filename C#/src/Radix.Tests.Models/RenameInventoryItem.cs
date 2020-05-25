using System;

namespace Radix.Tests.Models
{
    public class RenameInventoryItem : InventoryItemCommand
    {

        public RenameInventoryItem(string name) => Name = name;

        public string Name { get; }

        public int CompareTo(object obj) => throw new NotImplementedException();

        public int CompareTo(InventoryItemCommand other) => throw new NotImplementedException();

        public bool Equals(InventoryItemCommand other) => throw new NotImplementedException();
    }
}
