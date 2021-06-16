﻿using System;
using System.Collections.Generic;
using System.Text.Json;
using Radix.Inventory.Domain;
using static Radix.Option.Extensions;

namespace Radix.Tests
{
    public class TestSettings
    {

        public GarbageCollectionSettings CollectionSettings { get; } = new() {ScanInterval = TimeSpan.FromMilliseconds(500), IdleTimeout = TimeSpan.FromMilliseconds(500)};

        public Serialize<InventoryItemEvent, Json> SerializeEvent { get; } = input => new Json(JsonSerializer.Serialize(input));
        public Serialize<EventMetaData, Json> SerializeMetaData { get; } = json => new Json(JsonSerializer.Serialize(json));

        public FromEventDescriptor<InventoryItemEvent, Json> Descriptor { get; } = descriptor =>
        {
            if (string.Equals(descriptor.EventType.Value, nameof(InventoryItemCreated), StringComparison.Ordinal))
            {
                return Some(JsonSerializer.Deserialize<InventoryItemCreated>(descriptor.Event.Value));
            }

            if (string.Equals(descriptor.EventType.Value, nameof(InventoryItemDeactivated), StringComparison.Ordinal))
            {
                return Some(JsonSerializer.Deserialize<InventoryItemDeactivated>(descriptor.Event.Value));
            }

            if (string.Equals(descriptor.EventType.Value, nameof(InventoryItemRenamed), StringComparison.Ordinal))
            {
                return Some(JsonSerializer.Deserialize<InventoryItemRenamed>(descriptor.Event.Value));
            }

            if (string.Equals(descriptor.EventType.Value, nameof(InventoryItemRenamed), StringComparison.Ordinal))
            {
                return Some(JsonSerializer.Deserialize<InventoryItemRenamed>(descriptor.Event.Value));
            }

            return None<InventoryItemEvent>();
        };

        public ToTransientEventDescriptor<InventoryItemEvent, Json> ToTransientEventDescriptor { get; } = (messageId, @event, serialize, eventMetaData, serializeMetaData) =>
            new TransientEventDescriptor<Json>(new EventType(@event.GetType().Name), serialize(@event), serializeMetaData(eventMetaData), messageId);

#pragma warning disable 1998
        public async IAsyncEnumerable<EventDescriptor<InventoryItemEvent>> GetEventsSince(Id id, Version version, string streamIdentifier)
#pragma warning restore 1998
        {
            yield return new EventDescriptor<InventoryItemEvent>(
                new InventoryItemCreated(1, "Product 1", true, 1),
                1L,
                new EventType(typeof(InventoryItemCreated).FullName));
            yield return new EventDescriptor<InventoryItemEvent>(
                new ItemsCheckedInToInventory {Amount = 19, Id = 1},
                2L,
                new EventType(typeof(ItemsCheckedInToInventory).FullName));
            yield return new EventDescriptor<InventoryItemEvent>(
                new InventoryItemRenamed {Name = "Product 2", Id = 1},
                3L,
                new EventType(typeof(InventoryItemRenamed).FullName));
        }
    }
}
