using System.Text.Json.Serialization;

namespace Radix.Inventory.Domain.Data.Events;

[JsonDerivedType(typeof(ItemCreated), 0)]
[JsonDerivedType(typeof(ItemDeactivated), 1)]
[JsonDerivedType(typeof(ItemRenamed), 2)]
[JsonDerivedType(typeof(ItemsRemovedFromInventory), 3)]
[JsonDerivedType(typeof(ItemsCheckedInToInventory), 4)]
public record ItemEvent : InventoryEvent
{
}

public interface InventoryEvent
{
}
