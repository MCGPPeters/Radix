using System;
using System.Collections.Generic;

namespace Radix.Blazor.Inventory.Interface.Logic
{
    public struct InventoryItemModel
    {
        public Id id;
        public string name;
        public bool activated;

        public InventoryItemModel(Id id, string name, bool activated)
        {
            this.id = id;
            this.name = name;
            this.activated = activated;
        }

        public override bool Equals(object? obj)
        {
            return obj is InventoryItemModel other &&
                   EqualityComparer<Id>.Default.Equals(id, other.id) &&
                   name == other.name &&
                   activated == other.activated;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(id, name, activated);
        }

        public void Deconstruct(out Id id, out string name, out bool activated)
        {
            id = this.id;
            name = this.name;
            activated = this.activated;
        }

        public static implicit operator (Id id, string name, bool activated)(InventoryItemModel value)
        {
            return (value.id, value.name, value.activated);
        }

        public static implicit operator InventoryItemModel((Id id, string name, bool activated) value)
        {
            return new InventoryItemModel(value.id, value.name, value.activated);
        }
    }
}
