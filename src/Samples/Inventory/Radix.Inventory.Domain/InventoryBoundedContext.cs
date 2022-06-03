using System.Text.Json;
using Radix.Control.Nullable;
using SqlStreamStore;

namespace Radix.Inventory.Domain;

public class InventoryBoundedContext : BoundedContext<InventoryItemCommand, InventoryItemEvent, Json>
{
    public static readonly InMemoryStreamStore StreamStore = new();
    public static readonly SqlStreamStore SqlStreamStore = new(StreamStore);

    public AppendEvents<Json> AppendEvents => SqlStreamStore.AppendEvents;

    public GetEventsSince<InventoryItemEvent> GetEventsSince => SqlStreamStore.CreateGetEventsSince(
            (json, type) =>
            {
                if (string.Equals(type.Value, nameof(InventoryItemCreated), StringComparison.Ordinal))
                {
                    InventoryItemCreated? inventoryItemCreated = JsonSerializer.Deserialize<InventoryItemCreated>(json.Value);
                    return inventoryItemCreated.AsOption();
                }

                if (string.Equals(type.Value, nameof(InventoryItemDeactivated), StringComparison.Ordinal))
                {
                    InventoryItemDeactivated? inventoryItemDeactivated = JsonSerializer.Deserialize<InventoryItemDeactivated>(json.Value);
                    return inventoryItemDeactivated.AsOption();
                }


                if (string.Equals(type.Value, nameof(InventoryItemRenamed), StringComparison.Ordinal))
                {
                    InventoryItemRenamed? inventoryItemRenamed = JsonSerializer.Deserialize<InventoryItemRenamed>(json.Value);
                    return inventoryItemRenamed.AsOption();
                }

                return None<InventoryItemEvent>();
            },
            input =>
            {
                EventMetaData? eventMetaData = JsonSerializer.Deserialize<EventMetaData>(input.Value);
                return eventMetaData.AsOption();
            });

    public FromEventDescriptor<InventoryItemEvent, Json> FromEventDescriptor => descriptor =>
    {
        if (string.Equals(descriptor.EventType.Value, typeof(InventoryItemCreated).FullName, StringComparison.Ordinal))
        {
            InventoryItemCreated? inventoryItemCreated = JsonSerializer.Deserialize<InventoryItemCreated>(descriptor.Event.Value);
            return inventoryItemCreated.AsOption();
        }

        if (string.Equals(descriptor.EventType.Value, typeof(InventoryItemDeactivated).FullName, StringComparison.Ordinal))
        {
            InventoryItemDeactivated? inventoryItemDeactivated = JsonSerializer.Deserialize<InventoryItemDeactivated>(descriptor.Event.Value);
            return inventoryItemDeactivated.AsOption();
        }

        if (string.Equals(descriptor.EventType.Value, typeof(InventoryItemRenamed).FullName, StringComparison.Ordinal))
        {
            InventoryItemRenamed? inventoryItemRenamed = JsonSerializer.Deserialize<InventoryItemRenamed>(descriptor.Event.Value);
            return inventoryItemRenamed.AsOption();
        }

        return None<InventoryItemEvent>();
    };

    public Serialize<InventoryItemEvent, Json> Serialize => input =>
    {
        JsonSerializerOptions options = new() { Converters = { new PolymorphicWriteOnlyJsonConverter<InventoryItemEvent>() } };
        string jsonMessage = JsonSerializer.Serialize(input, options);
        return new Json(jsonMessage);
    };

    public Serialize<EventMetaData, Json> SerializeMetaData => json => new Json(JsonSerializer.Serialize(json));

}
