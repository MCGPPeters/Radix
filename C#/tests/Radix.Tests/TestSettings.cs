using System;
using System.Collections.Generic;
using System.Text.Json;
using Radix.Tests.Models;

namespace Radix.Tests
{
    public class TestSettings
    {

        public GarbageCollectionSettings CollectionSettings { get; } = new GarbageCollectionSettings
        {
            ScanInterval = TimeSpan.FromMilliseconds(500), IdleTimeout = TimeSpan.FromMilliseconds(500)
        };

        public Serialize<InventoryItemEvent, Json> SerializeEvent { get; } = input => new Json(JsonSerializer.Serialize(input));
        public Serialize<EventMetaData, Json> SerializeMetaData { get; } = json => new Json(JsonSerializer.Serialize(json));

        public FromEventDescriptor<InventoryItemEvent, Json> Descriptor { get; } = (parseEvent, parseMetaData, descriptor) =>
        {
            if (string.Equals(descriptor.EventType.Value, nameof(InventoryItemCreated), StringComparison.Ordinal))
            {
                return JsonSerializer.Deserialize<InventoryItemCreated>(descriptor.Event.Value);
            }

            if (string.Equals(descriptor.EventType.Value, nameof(InventoryItemDeactivated), StringComparison.Ordinal))
            {
                return JsonSerializer.Deserialize<InventoryItemDeactivated>(descriptor.Event.Value);
            }

            if (string.Equals(descriptor.EventType.Value, nameof(InventoryItemRenamed), StringComparison.Ordinal))
            {
                return JsonSerializer.Deserialize<InventoryItemRenamed>(descriptor.Event.Value);
            }

            if (string.Equals(descriptor.EventType.Value, nameof(InventoryItemRenamed), StringComparison.Ordinal))
            {
                return JsonSerializer.Deserialize<InventoryItemRenamed>(descriptor.Event.Value);
            }

            throw new InvalidOperationException("Unknown event");
        };

        public ToTransientEventDescriptor<InventoryItemEvent, Json> ToTransientEventDescriptor { get; } = (@event, serialize, eventMetaData, serializeMetaData) =>
            new TransientEventDescriptor<Json>(new EventType(@event.GetType().Name), serialize(@event), serializeMetaData(eventMetaData));

        public static async IAsyncEnumerable<EventDescriptor<Json>> GetEventsSince(Address address, Version version, string streamIdentifier)
        {
            MessageId messageId = new MessageId(new Guid());
            yield return new EventDescriptor<Json>(
                address,
                new Json(JsonSerializer.Serialize(new EventMetaData(messageId, messageId))),
                new Json(JsonSerializer.Serialize(new InventoryItemCreated(1, "Product 1", true, 0))),
                1L,
                new EventType(nameof(InventoryItemCreated)));
            yield return new EventDescriptor<Json>(
                address,
                new Json(JsonSerializer.Serialize(new EventMetaData(messageId, messageId))),
                new Json(JsonSerializer.Serialize(new ItemsCheckedInToInventory(19))),
                2L,
                new EventType(nameof(ItemsCheckedInToInventory)));
            yield return new EventDescriptor<Json>(
                address,
                new Json(JsonSerializer.Serialize(new EventMetaData(messageId, messageId))),
                new Json(JsonSerializer.Serialize(new InventoryItemRenamed("Product 2"))),
                3L,
                new EventType(nameof(InventoryItemRenamed)));
        }
    }
}
