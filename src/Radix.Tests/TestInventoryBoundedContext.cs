using System.Text.Json;
using Radix.Control.Nullable;
using Radix.Domain.Control;
using Radix.Domain.Data;
using Radix.Inventory.Domain;
using Radix.Inventory.Domain.Data.Commands;
using Radix.Inventory.Domain.Data.Events;
using Version = System.Version;

namespace Radix.Tests;
public class TestInventoryBoundedContext : Context<ItemCommand, ItemEvent, Json>
{
    static long existingVersion = 3L; // this is the initial number of events in the fake event store

    public async IAsyncEnumerable<EventDescriptor<ItemEvent>> GetEventsSinceInternal(Radix.Domain.Data.Aggregate.Id id, Radix.Domain.Data.Version version, string streamIdentifier)
#pragma warning restore 1998
    {
        yield return new EventDescriptor<ItemEvent>(
            new ItemCreated(1, "Product 1", true, 1),
            1L,
            new EventType(typeof(ItemCreated).FullName ?? throw new InvalidOperationException()));
        yield return new EventDescriptor<ItemEvent>(
            new ItemsCheckedInToInventory { Amount = 19, Id = 1 },
            2L,
            new EventType(typeof(ItemsCheckedInToInventory).FullName ?? throw new InvalidOperationException()));
        yield return new EventDescriptor<ItemEvent>(
            new ItemRenamed { Name = "Product 2", Id = 1 },
            3L,
            new EventType(typeof(ItemRenamed).FullName ?? throw new InvalidOperationException()));
    }

    public GetEventsSince<ItemEvent> GetEventsSince => GetEventsSinceInternal;

    public Serialize<ItemEvent, Json> Serialize => input =>
    {
        JsonSerializerOptions options = new() { Converters = { new PolymorphicWriteOnlyJsonConverter<ItemEvent>() } };
        string jsonMessage = JsonSerializer.Serialize(input, options);
        return new Json(jsonMessage);
    };

    public Serialize<EventMetaData, Json> SerializeMetaData => json => new Json(JsonSerializer.Serialize(json));


    public AppendEvents<Json> AppendEvents => (_, _, _, _) => Task.FromResult(Ok<ExistingVersion, AppendEventsError>(++existingVersion));

}
