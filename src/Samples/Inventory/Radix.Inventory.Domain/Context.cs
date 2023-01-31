using System.Text.Json;
using Radix.Control.Nullable;
using Radix.Domain.Data;
using Radix.Inventory.Domain.Data.Commands;
using Radix.Inventory.Domain.Data.Events;
using SqlStreamStore;

namespace Radix.Inventory.Domain;

public class Context : Context<ItemCommand, ItemEvent, Json>
{
    public static readonly InMemoryStreamStore StreamStore = new();
    public static readonly Radix.Domain.Data.SqlStreamStore SqlStreamStore = new(StreamStore);

    public AppendEvents<Json> AppendEvents => SqlStreamStore.AppendEvents;

    public GetEventsSince<ItemEvent> GetEventsSince => SqlStreamStore.CreateGetEventsSince<ItemEvent>();

    public FromEventDescriptor<ItemEvent, Json> FromEventDescriptor => descriptor =>
    {
        if (string.Equals(descriptor.EventType.Value, typeof(ItemCreated).FullName, StringComparison.Ordinal))
        {
            ItemCreated? inventoryItemCreated = JsonSerializer.Deserialize<ItemCreated>(descriptor.Event.Value);
            return inventoryItemCreated.AsOption();
        }

        if (string.Equals(descriptor.EventType.Value, typeof(ItemDeactivated).FullName, StringComparison.Ordinal))
        {
            ItemDeactivated? inventoryItemDeactivated = JsonSerializer.Deserialize<ItemDeactivated>(descriptor.Event.Value);
            return inventoryItemDeactivated.AsOption();
        }

        if (string.Equals(descriptor.EventType.Value, typeof(ItemRenamed).FullName, StringComparison.Ordinal))
        {
            ItemRenamed? inventoryItemRenamed = JsonSerializer.Deserialize<ItemRenamed>(descriptor.Event.Value);
            return inventoryItemRenamed.AsOption();
        }

        return None<ItemEvent>();
    };

    public Serialize<ItemEvent, Json> Serialize => input =>
    {
        JsonSerializerOptions options = new() { Converters = { new PolymorphicWriteOnlyJsonConverter<ItemEvent>() } };
        string jsonMessage = JsonSerializer.Serialize(input, options);
        return new Json(jsonMessage);
    };

    public Serialize<EventMetaData, Json> SerializeMetaData => json => new Json(JsonSerializer.Serialize(json));

}
