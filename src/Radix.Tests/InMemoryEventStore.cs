﻿using System.Text.Json;
using Radix.Control.Task.Result;
using Radix.Data;
using Radix.Domain.Data;
using Radix.Inventory.Domain.Data.Events;
using Radix.Math.Pure.Logic.Order.Intervals;
using Version = Radix.Domain.Data.Version;

namespace Radix.Tests;

public class InMemoryEventStore : EventStore<InMemoryEventStore>
{
    private static readonly Dictionary<string, List<string>> serializedEvents = new() { };
    public static long CurrentVersion = 0;

    public static JsonSerializerOptions Options = new() { Converters = { new PolymorphicWriteOnlyJsonConverter<InventoryEvent>() } };

    public static Task<Result<ExistingVersion, AppendEventsError>> AppendEvents<TEvent>(string streamName,
        Version expectedVersion, params TEvent[] events)
    {
        foreach (TEvent @event in events)
        {
            if (serializedEvents.ContainsKey(streamName))
            {
                serializedEvents[streamName].Add(JsonSerializer.Serialize(@event, Options));
            }
            else
            {
                serializedEvents.Add(streamName, new List<string>(){ JsonSerializer.Serialize(@event, Options) });
            }
            CurrentVersion++;
        }

        return new ExistingVersion(CurrentVersion).Return<ExistingVersion, AppendEventsError>();
    }

    public static TEvent Deserialize<TEvent>(string json) => JsonSerializer.Deserialize<TEvent>(json, Options);
    public static async IAsyncEnumerable<Event<TEvent>> GetEvents<TEvent>(string streamName, Closed<Version> interval)

    {
        long version = 0;
        foreach ( (string streamId, List<string> serializedEvents) in serializedEvents.Where(kvp => kvp.Key == streamName))
        {
            foreach (string serializedEvent in serializedEvents)
            {
                version++;
                yield return new Event<TEvent>
                {
                    Value = Deserialize<TEvent>(serializedEvent),
                    EventType = typeof(TEvent).FullName,
                    Version = new ExistingVersion(version)
                };
            }
            
        }
    }
}