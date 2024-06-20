using System.Text.Json;
using EventStore.Client;
using Radix.Data;
using Radix.Domain.Data;
using Radix.Math.Pure.Logic.Order.Intervals;
using static Radix.Control.Result.Extensions;

namespace Radix.Domain.EventStoreDB;

public class EventStore : EventStore<EventStore, EventStoreClientSettings>
{
    public record Metadata(TenantId TenantId);

    public static async Task<Result<ExistingVersion, AppendEventsError>> AppendEvents<TEvent>(
        EventStoreClientSettings eventStoreSettings, TenantId tenantId, Data.Stream stream,
        Data.Version expectedVersion, params TEvent[] events)
    {
        var client = new EventStoreClient(eventStoreSettings);

        var result = await client.AppendToStreamAsync(
            stream.ToString(),
            StreamState.Any,
            events.Select(@event => new EventData(
                new Uuid(),
                @event.GetType().Name,
                JsonSerializer.SerializeToUtf8Bytes(@event),
                JsonSerializer.SerializeToUtf8Bytes(new Metadata(tenantId)))));

        return result switch
        {
            SuccessResult successResult => Ok<ExistingVersion, AppendEventsError>(new ExistingVersion(successResult.NextExpectedVersion)),
            WrongExpectedVersionResult wrongExpectedVersionResult => Error<ExistingVersion, AppendEventsError>(new OptimisticConcurrencyError
            {
                Message = $"Expected the version of the aggregate to be {expectedVersion} however it was actually {wrongExpectedVersionResult.ActualVersion}"
            }),
            _ => throw new NotSupportedException()
        }; 
    }
    public static async IAsyncEnumerable<Event<TEvent>> GetEvents<TEvent>(EventStoreClientSettings eventStoreSettings,
        TenantId tenantId, Data.Stream stream,
        Closed<Data.Version> interval)
    { 
        var client = new EventStoreClient(eventStoreSettings);

        var result = client.ReadStreamAsync(Direction.Forwards, stream.ToString(),
            new StreamPosition((ulong)interval.LowerBound.Value));

        await foreach (var @event in result)
        {
            var metadata = JsonSerializer.Deserialize<Metadata>(@event.Event.Metadata.Span);

            if (metadata.TenantId != tenantId)
            {
                throw new Exception("Event metadata is invalid");
            }

            yield return new Event<TEvent>
            {
                EventType = @event.Event.EventType,
                Value = JsonSerializer.Deserialize<TEvent>(@event.Event.Data.Span),
                Version = new ExistingVersion(@event.OriginalEventNumber.ToInt64())
            };
        }
    }
}
