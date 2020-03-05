using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
// using EventStore.Client;
using Radix.Async;
using static Radix.Option.Extensions;
using Radix.Tests.Models;
using SqlStreamStore;
using SqlStreamStore.Streams;
using Extensions = Radix.Result.Extensions;

namespace Radix.Blazor.Sample
{
    public static class SqlStreamStore
    {

        private static readonly IStreamStore streamStore = new InMemoryStreamStore();

        // private static readonly EventStoreClient client = new EventStoreClient(new EventStoreClientSettings(new Uri("tcp://admin:changeit@localhost:1113")));
        //
        public static AppendEvents<InventoryItemEvent> AppendEvents => async (address, version, events) =>
        {
            var newStreamMessages = events.Select(
                inventoryItemEvent =>
                {
                    var jsonMessage = JsonSerializer.Serialize(inventoryItemEvent);
                    var messageId = inventoryItemEvent.Address.Value;
                    var typeName = inventoryItemEvent.GetType().ToString();
                    var eventType = char.ToLowerInvariant(typeName[0]) + typeName.Substring(1);
                    return new NewStreamMessage(messageId, eventType, jsonMessage);
                }).ToArray();

            Func<Task<AppendResult>> appendToStream;
            AppendResult result;
            var streamId = $"InventoryItem-{address.ToString()}";

            switch (version)
            {
                case AnyVersion _:
                    appendToStream = () => streamStore.AppendToStream(streamId, ExpectedVersion.Any, newStreamMessages);
                    result = await appendToStream.Retry(Backoff.Exponentially());
                    return Extensions.Ok<Version, SaveEventsError>(result.CurrentVersion);
                case Version v:
                    var expectedVersion = Convert.ToInt32(v.Value);
                    appendToStream = () => streamStore.AppendToStream(streamId, expectedVersion, newStreamMessages);
                    result = await appendToStream.Retry(Backoff.Exponentially());
                    return Extensions.Ok<Version, SaveEventsError>(result.CurrentVersion);
                default:
                    throw new NotSupportedException("Unknown type of version");
            }
        };

        public static GetEventsSince<InventoryItemEvent> GetEventsSince => EventsSince;

        private static async IAsyncEnumerable<EventDescriptor<InventoryItemEvent>> EventsSince(Address address, IVersion version)
        {
            var streamId = $"InventoryItem-{address.ToString()}";
            switch (version)
            {
                case Version v:
                    // IAsyncEnumerable<ReadStreamPage> readAllEventsForwardAsync =
                    var fromVersionInclusive = Convert.ToInt32(v.Value);
                    var readStreamPage = await streamStore.ReadStreamForwards(streamId, fromVersionInclusive, Int32.MaxValue, false);

                    var eventDescriptors = readStreamPage.Messages.Select(async streamMessage =>
                    {
                        var inventoryItemEvent = JsonSerializer.Deserialize<InventoryItemEvent>(await streamMessage.GetJsonData());
                        return new EventDescriptor<InventoryItemEvent>(inventoryItemEvent, streamMessage.StreamVersion);
                    });

                    foreach (var eventDescriptor in eventDescriptors)
                    {
                        yield return await eventDescriptor;
                    }

                    break;
                default:
                    throw new NotSupportedException("Unknown type of version");
            }
        }

        public static ResolveRemoteAddress ResolveRemoteAddress => address => Task.FromResult(Extensions.Ok<Uri, ResolveRemoteAddressError>(new Uri("")));

        public static Forward<InventoryItemCommand> Forward => (_, __, ___) => Task.FromResult(Extensions.Ok<Unit, ForwardError>(Unit.Instance));
        public static FindConflict<InventoryItemCommand, InventoryItemEvent> FindConflict => (_, __) => None<Conflict<InventoryItemCommand, InventoryItemEvent>>();

        public static OnConflictingCommandRejected<InventoryItemCommand, InventoryItemEvent> onConflictingCommandRejected => (conflicts) => Task.FromResult(Unit.Instance);
        //
        //
        public static BoundedContext<InventoryItemCommand, InventoryItemEvent> Create() =>
            new BoundedContext<InventoryItemCommand, InventoryItemEvent>(new BoundedContextSettings<InventoryItemCommand, InventoryItemEvent>(AppendEvents, GetEventsSince, ResolveRemoteAddress, Forward, FindConflict, onConflictingCommandRejected, new GarbageCollectionSettings()));
    }
}

