using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using EventStore.Client;
using Radix.Async;
using Radix.Tests.Models;
using Extensions = Radix.Result.Extensions;

namespace Radix.Blazor.Sample
{
    public static class BoundedContext
    {

        private static readonly EventStoreClient client = new EventStoreClient(new EventStoreClientSettings(new Uri("tcp://admin:changeit@localhost:1113")));
//
        public static SaveEvents<InventoryItemEvent> SaveEvents => async (address, version, events) =>
        {

            var eventData = events.Select(
                @event =>
                {
                    var typeName = @event.GetType().ToString();
                    var eventType = char.ToLowerInvariant(typeName[0]) + typeName.Substring(1);
                    var eventAsJSON = JsonSerializer.SerializeToUtf8Bytes(@event);
                    return new EventData(Uuid.FromGuid(@event.Address.Value), eventType,eventAsJSON, Array.Empty<byte>());
                }).ToArray();

            Func<Task<WriteResult>> appendToStream;

            switch (version)
            {
                case AnyVersion _:
                    appendToStream = () => client.AppendToStreamAsync($"InventoryItem-{address.ToString()}", AnyStreamRevision.Any, eventData);
                    var result = await appendToStream.Retry(Backoff.Exponentially());
                    return Extensions.Ok<Version, SaveEventsError>(result.NextExpectedVersion);
                case Version v:
                    appendToStream = () => client.AppendToStreamAsync($"InventoryItem-{address.ToString()}", StreamRevision.FromInt64(v.Value), eventData);
                    result = await appendToStream.Retry(Backoff.Exponentially());
                    return Extensions.Ok<Version, SaveEventsError>(result.NextExpectedVersion);
                default:
                    throw new NotSupportedException("Unknown type of version");
            }
        };

        public static GetEventsSince<InventoryItemEvent> GetEventsSince => (address, version) =>
        {
            switch (version)
            {
                case Version v:
                    IAsyncEnumerable<ResolvedEvent> readAllEventsForwardAsync =
                        client.ReadStreamAsync(Direction.Forwards, $"InventoryItem-{address.ToString()}",
                            StreamRevision.FromInt64(v.Value), Int32.MaxValue, false);

                    return readAllEventsForwardAsync.Select(
                        resolvedEvent =>
                        {
                            var inventoryItemEvent = JsonSerializer.Deserialize<InventoryItemEvent>(resolvedEvent.Event.Data);
                            return new EventDescriptor<InventoryItemEvent>(inventoryItemEvent, resolvedEvent.Event.EventNumber.ToInt64());
                        });
                
                default:
                    throw new NotSupportedException("Unknown type of version");
            }
        };

        public static ResolveRemoteAddress ResolveRemoteAddress => address => Task.FromResult(Extensions.Ok<Uri, ResolveRemoteAddressError>(new Uri("")));

        public static Forward<InventoryItemCommand> Forward => (_, __, ___) => Task.FromResult(Extensions.Ok<Unit, ForwardError>(Unit.Instance));
        public static FindConflicts<InventoryItemCommand, InventoryItemEvent> FindConflicts => (_, __) => Enumerable.Empty<Conflict<InventoryItemCommand, InventoryItemEvent>>().ToAsyncEnumerable();

        public static OnConflictingCommandRejected<InventoryItemCommand, InventoryItemEvent> onConflictingCommandRejected => (conflicts) => Task.FromResult(Unit.Instance);
//
//
        public static BoundedContext<InventoryItemCommand, InventoryItemEvent> Create() =>
            new BoundedContext<InventoryItemCommand, InventoryItemEvent>(new BoundedContextSettings<InventoryItemCommand, InventoryItemEvent>(SaveEvents, GetEventsSince, ResolveRemoteAddress, Forward, FindConflicts, onConflictingCommandRejected, new GarbageCollectionSettings()));
    }
}

