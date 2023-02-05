using System.Text.Json;
using Radix.Control.Task.Result;
using Radix.Data;
using Radix.Domain.Data;
using Radix.Inventory.Domain.Data.Events;
using Radix.Math.Pure.Logic.Order.Intervals;
using Stream = Radix.Domain.Data.Stream;
using Version = Radix.Domain.Data.Version;

namespace Radix.Inventory.Domain;

public class InMemoryEventStore: EventStore<InMemoryEventStore>
{
    private static readonly List<string> serializedEvents = new() { };
    public static long CurrentVersion = 0;

    public static JsonSerializerOptions Options = new() { Converters = { new PolymorphicWriteOnlyJsonConverter<InventoryEvent>() } };

    public static Task<Result<ExistingVersion, AppendEventsError>> AppendEvents<TEvent>(Stream eventStream,
        Version expectedVersion, params TEvent[] events)
    {
        foreach (TEvent @event in events)
        {
            serializedEvents.Add(JsonSerializer.Serialize(@event, Options));
            CurrentVersion++;
        }

        return new ExistingVersion(CurrentVersion).Return<ExistingVersion, AppendEventsError>();
    }

    public static TEvent Deserialize<TEvent>(string json) => JsonSerializer.Deserialize<TEvent>(json, Options)!;
    public static async IAsyncEnumerable<Event<TEvent>> GetEvents<TEvent>(Stream eventStream, Closed<Version> interval)

    {
        long version = 0;
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
