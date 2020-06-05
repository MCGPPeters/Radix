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

        public ToTransientEventDescriptor<InventoryItemEvent, Json> ToTransientEventDescriptor { get; } = (messageId, @event, serialize, eventMetaData, serializeMetaData) =>
            new TransientEventDescriptor<Json>(new EventType(@event.GetType().Name), serialize(@event), serializeMetaData(eventMetaData), messageId);

#pragma warning disable 1998
        public async IAsyncEnumerable<EventDescriptor<Json>> GetEventsSince(Address address, Version version, string streamIdentifier)
#pragma warning restore 1998
        {
            yield return new EventDescriptor<Json>(
                new Json(JsonSerializer.Serialize(new InventoryItemCreated {Name = "Product 1", Activated = true, Count = 1})),
                1L,
                new EventType(nameof(InventoryItemCreated)));
            yield return new EventDescriptor<Json>(
                new Json(JsonSerializer.Serialize(new ItemsCheckedInToInventory {Amount = 19})),
                2L,
                new EventType(nameof(ItemsCheckedInToInventory)));
            yield return new EventDescriptor<Json>(
                new Json(JsonSerializer.Serialize(new InventoryItemRenamed {Name = "Product 2"})),
                3L,
                new EventType(nameof(InventoryItemRenamed)));
        }
    }
}
