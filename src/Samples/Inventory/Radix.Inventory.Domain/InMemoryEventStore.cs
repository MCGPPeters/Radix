using System.Collections.Concurrent;
using System.Text.Json;
using Radix.Control.Task.Result;
using Radix.Data;
using Radix.Domain.Data;
using Radix.Inventory.Domain.Data.Events;
using Radix.Math.Pure.Logic.Order.Intervals;
using Stream = Radix.Domain.Data.Stream;
using Version = Radix.Domain.Data.Version;

namespace Radix.Inventory.Domain;

public class InMemoryEventStore : EventStore<InMemoryEventStore, InMemoryEventStoreSettings>
{

    public static TEvent Deserialize<TEvent>(string json) => JsonSerializer.Deserialize<TEvent>(json)!;

    public static Task<Result<ExistingVersion, AppendEventsError>> AppendEvents<TEvent>(InMemoryEventStoreSettings eventStoreSettings, TenantId tenantId, Stream stream,
        Version expectedVersion, params TEvent[] events)
    {
        foreach (TEvent @event in events)
        {
            if (eventStoreSettings.SerializedEvents.ContainsKey(stream.Name.ToString()))
            {
                eventStoreSettings.SerializedEvents[stream.Name.ToString()].Add(JsonSerializer.Serialize(value: @event));
            }
            else
            {
                eventStoreSettings.SerializedEvents.TryAdd(key: stream.Name.ToString(), new List<string>() { JsonSerializer.Serialize(value: @event) });
            }
            eventStoreSettings.CurrentVersion++;
        }

        return new ExistingVersion(eventStoreSettings.CurrentVersion).Return<ExistingVersion, AppendEventsError>();
    }

    public static async IAsyncEnumerable<Event<TEvent>> GetEvents<TEvent>(InMemoryEventStoreSettings eventStoreSettings,
        TenantId tenantId,
        Stream stream, Closed<Version> interval)
    {
        long version = 0;
        foreach ((string streamId, List<string> serializedEvents) in eventStoreSettings.SerializedEvents.Where(kvp => kvp.Key == stream.Name.ToString()))
        {
            foreach (string serializedEvent in serializedEvents)
            {
                version++;
                yield return new Event<TEvent>
                {
                    Value = Deserialize<TEvent>(serializedEvent),
                    EventType = typeof(TEvent).FullName!,
                    Version = new ExistingVersion(version)
                };
            }

        }
    }
}


public class InMemoryEventStoreSettings
{

    public ConcurrentDictionary<string, List<string>> SerializedEvents = new() { };
    public long CurrentVersion {get; set; }}
}

