using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Radix.Async;
using SqlStreamStore;
using SqlStreamStore.Streams;
using Extensions = Radix.Result.Extensions;

namespace Radix
{

    public class SqlStreamStore<TEvent> : EventStore<TEvent> where TEvent : Event
    {

        private static readonly IStreamStore streamStore = new InMemoryStreamStore();

        public AppendEvents<TEvent> AppendEvents =>
            async (address, version, events) =>
            {
                NewStreamMessage[] newStreamMessages = events.Select(
                    inventoryItemEvent =>
                    {
                        string jsonMessage = JsonSerializer.Serialize(inventoryItemEvent);
                        Guid messageId = inventoryItemEvent.Event.Aggregate.Value;
                        return new NewStreamMessage(messageId, inventoryItemEvent.ToString(), jsonMessage);
                    }).ToArray();

                Func<Task<AppendResult>> appendToStream;
                AppendResult result;
                string streamId = $"InventoryItem-{address.ToString()}";

                switch (version)
                {
                    case AnyVersion _:
                        appendToStream = () => streamStore.AppendToStream(streamId, ExpectedVersion.Any, newStreamMessages);
                        result = await appendToStream.Retry(Backoff.Exponentially());
                        return Extensions.Ok<Version, AppendEventsError>(result.CurrentVersion);
                    case Version v:
                        int expectedVersion = Convert.ToInt32(v.Value);
                        appendToStream = () => streamStore.AppendToStream(streamId, expectedVersion, newStreamMessages);
                        result = await appendToStream.Retry(Backoff.Exponentially());
                        return Extensions.Ok<Version, AppendEventsError>(result.CurrentVersion);
                    default:
                        throw new NotSupportedException("Unknown type of version");
                }
            };

        public GetEventsSince<TEvent> GetEventsSince => (address, version) => EventsSince(address, version, $"InventoryItem-{address.ToString()}");

        private async IAsyncEnumerable<EventDescriptor<TEvent>> EventsSince(Address address, IVersion version, string streamId)
        {
            switch (version)
            {
                case Version v:
                    // IAsyncEnumerable<ReadStreamPage> readAllEventsForwardAsync =
                    int fromVersionInclusive = Convert.ToInt32(v.Value);
                    ReadStreamPage readStreamPage = await streamStore.ReadStreamForwards(streamId, fromVersionInclusive, int.MaxValue, false);

                    IEnumerable<Task<EventDescriptor<TEvent>>> eventDescriptors = readStreamPage.Messages.Select(
                        async streamMessage =>
                        {
                            TransientEventDescriptor<TEvent> @event = JsonSerializer.Deserialize<TransientEventDescriptor<TEvent>>(await streamMessage.GetJsonData());
                            return new EventDescriptor<TEvent>(
                                @event.Event.Aggregate,
                                @event.MessageId,
                                @event.CausationId,
                                @event.CorrelationId,
                                @event.Event,
                                streamMessage.StreamVersion);
                        });

                    foreach (Task<EventDescriptor<TEvent>> eventDescriptor in eventDescriptors)
                    {
                        yield return await eventDescriptor;
                    }

                    break;
                default:
                    throw new NotSupportedException("Unknown type of version");
            }
        }
    }
}
