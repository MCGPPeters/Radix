using System.Text.Json;
using Radix.Control.Nullable;
using Radix.Inventory.Domain;

namespace Radix.Tests
{
    public class TestInventoryBoundedContext : BoundedContext<InventoryItemCommand, InventoryItemEvent, Json>
    {
        static long existingVersion = 3L; // this is the initial number of events in the fake event store


        public async IAsyncEnumerable<EventDescriptor<InventoryItemEvent>> GetEventsSinceInternal(Id id, Version version, string streamIdentifier)
#pragma warning restore 1998
        {
            yield return new EventDescriptor<InventoryItemEvent>(
                new InventoryItemCreated(1, "Product 1", true, 1),
                1L,
                new EventType(typeof(InventoryItemCreated).FullName ?? throw new InvalidOperationException()));
            yield return new EventDescriptor<InventoryItemEvent>(
                new ItemsCheckedInToInventory { Amount = 19, Id = 1 },
                2L,
                new EventType(typeof(ItemsCheckedInToInventory).FullName ?? throw new InvalidOperationException()));
            yield return new EventDescriptor<InventoryItemEvent>(
                new InventoryItemRenamed { Name = "Product 2", Id = 1 },
                3L,
                new EventType(typeof(InventoryItemRenamed).FullName ?? throw new InvalidOperationException()));
        }

        public GetEventsSince<InventoryItemEvent> GetEventsSince => GetEventsSinceInternal;

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


        public AppendEvents<Json> AppendEvents => (_, _, _, _) => Task.FromResult(Ok<ExistingVersion, AppendEventsError>(++existingVersion));

    }
}
