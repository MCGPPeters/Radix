using System.Text.Json;
using Radix.Control.Nullable;
using Radix.Data;
using Radix.Domain.Data;
using SqlStreamStore;

namespace Radix.Inventory.Pages;

public class CounterContext : Context<IncrementCommand, CounterIncremented, Json>
{
    public static readonly InMemoryStreamStore StreamStore = new();
    public readonly Radix.Domain.Data.SqlStreamStore SqlStreamStore = new(StreamStore);

    public AppendEvents<Json> AppendEvents => SqlStreamStore.AppendEvents;

    
    public GetEventsSince<CounterIncremented> GetEventsSince => SqlStreamStore.CreateGetEventsSince<CounterIncremented>();

    private static Option<EventMetaData> ParseMetaData(Json input)
    {
        EventMetaData? eventMetaData = JsonSerializer.Deserialize<EventMetaData>(input.Value);
        return eventMetaData.AsOption();
    }

    public FromEventDescriptor<CounterIncremented, Json> FromEventDescriptor => descriptor => Some(new CounterIncremented());
    public Serialize<CounterIncremented, Json> Serialize => input => new Json(JsonSerializer.Serialize(input));

    public Serialize<EventMetaData, Json> SerializeMetaData => input => new Json(JsonSerializer.Serialize(input));
}
