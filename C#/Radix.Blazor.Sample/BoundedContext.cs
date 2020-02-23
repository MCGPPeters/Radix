using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using EventStore.ClientAPI;
using Radix.Async;
using Radix.Tests.Models;
using Extensions = Radix.Result.Extensions;

namespace Radix.Blazor.Sample
{
    public static class BoundedContext
    {

        private static readonly IEventStoreConnection eventStoreConnection = EventStoreConnection.Create(new Uri("tcp://admin:changeit@localhost:1113"));

        public static SaveEvents<InventoryItemEvent> SaveEvents => async (address, version, events) =>
        {

            var eventData = events.Select(
                @event =>
                {
                    var typeName = @event.GetType().ToString();
                    var eventType = char.ToLowerInvariant(typeName[0]) + typeName.Substring(1);
                    var eventAsJSON = JsonSerializer.SerializeToUtf8Bytes<InventoryItemEvent>(@event);
                    return new EventData(@event.Address.Value, eventType, true, eventAsJSON, Array.Empty<byte>());
                }).ToArray();

            Func<Task<WriteResult>> appendToStream;

            switch (version)
            {
                case AnyVersion anyVersion:
                    appendToStream = () => eventStoreConnection.AppendToStreamAsync($"InventoryItem-{address.ToString()}", anyVersion.Value, eventData);
                    var result = await appendToStream.Retry(Backoff.Exponentially());
                    return Extensions.Ok<Version, SaveEventsError>(result.NextExpectedVersion);
                case Version v:
                    appendToStream = () => eventStoreConnection.AppendToStreamAsync($"InventoryItem-{address.ToString()}", v.Value, eventData);
                    result = await appendToStream.Retry(Backoff.Exponentially());
                    return Extensions.Ok<Version, SaveEventsError>(result.NextExpectedVersion);
                default:
                    throw new NotSupportedException("Unknown type of version");
            }
        };

        public static GetEventsSince<InventoryItemEvent> GetEventsSince => async (address, version) =>
        {
            switch (version)
            {
                case AnyVersion anyVersion:
                    Func<Task<StreamEventsSlice>> readAllEventsForwardAsync = () => eventStoreConnection.ReadStreamEventsForwardAsync(address.ToString(), anyVersion.Value, Int32.MaxValue, false);
                    var result = await readAllEventsForwardAsync.Retry(Backoff.Exponentially());
                    return result.Events.Select(
                        resolvedEvent =>
                        {
                            var inventoryItemEvent = JsonSerializer.Deserialize<InventoryItemEvent>(resolvedEvent.Event.Data);
                            return new EventDescriptor<InventoryItemEvent>(inventoryItemEvent, resolvedEvent.OriginalEventNumber);
                        });
                case Version v:
                    readAllEventsForwardAsync = () => eventStoreConnection.ReadStreamEventsForwardAsync(address.ToString(), v.Value, Int32.MaxValue, false);
                    result = await readAllEventsForwardAsync.Retry(Backoff.Exponentially());
                    return result.Events.Select(
                        resolvedEvent =>
                        {
                            var inventoryItemEvent = JsonSerializer.Deserialize<InventoryItemEvent>(resolvedEvent.Event.Data);
                            return new EventDescriptor<InventoryItemEvent>(inventoryItemEvent, resolvedEvent.OriginalEventNumber);
                        });
                default:
                    throw new NotSupportedException("Unknown type of version");
            }
        };

        public static ResolveRemoteAddress ResolveRemoteAddress => address => Task.FromResult(Extensions.Ok<Uri, ResolveRemoteAddressError>(new Uri("")));

        public static Forward<InventoryItemCommand> Forward => (_, __, ___) => Task.FromResult(Extensions.Ok<Unit, ForwardError>(Unit.Instance));
        public static FindConflicts<InventoryItemCommand, InventoryItemEvent> FindConflicts => (_, __) => Enumerable.Empty<Conflict<InventoryItemCommand, InventoryItemEvent>>();

        public static OnConflictingCommandRejected<InventoryItemCommand, InventoryItemEvent> onConflictingCommandRejected => (conflicts) => Task.FromResult(Unit.Instance);


        public static BoundedContext<InventoryItemCommand, InventoryItemEvent> Create() =>
            new BoundedContext<InventoryItemCommand, InventoryItemEvent>(new BoundedContextSettings<InventoryItemCommand, InventoryItemEvent>(SaveEvents, GetEventsSince, ResolveRemoteAddress, Forward, FindConflicts, onConflictingCommandRejected, new GarbageCollectionSettings()));
    }
}
