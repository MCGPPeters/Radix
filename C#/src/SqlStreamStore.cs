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

    public class SqlStreamStore<TEvent> where TEvent : Event
    {

        private static readonly IStreamStore _streamStore = new InMemoryStreamStore();

        public static AppendEvents<TEvent> AppendEvents =>
            async (address, version, streamIdentifier, events) =>
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
                int expectedVersion;

                switch (version)
                {
                    case AnyVersion _:
                        appendToStream = () => _streamStore.AppendToStream(streamIdentifier, ExpectedVersion.Any, newStreamMessages);
                        result = await appendToStream.Retry(Backoff.Exponentially());
                        return Extensions.Ok<ExistentVersion, AppendEventsError>(result.CurrentVersion);
                    case NoneExistentVersion _:
                        expectedVersion = ExpectedVersion.NoStream;
                        appendToStream = () => _streamStore.AppendToStream(streamIdentifier, expectedVersion, newStreamMessages);
                        result = await appendToStream.Retry(Backoff.Exponentially());
                        return Extensions.Ok<ExistentVersion, AppendEventsError>(result.CurrentVersion);
                    case ExistentVersion existentVersion:
                        expectedVersion = Convert.ToInt32(existentVersion.Value);
                        appendToStream = () => _streamStore.AppendToStream(streamIdentifier, expectedVersion, newStreamMessages);
                        result = await appendToStream.Retry(Backoff.Exponentially());
                        return Extensions.Ok<ExistentVersion, AppendEventsError>(result.CurrentVersion);
                    default:
                        throw new NotSupportedException("Unknown type of existentVersion");
                }
            };

        public static GetEventsSince<TEvent> GetEventsSince => EventsSince;

        private static async IAsyncEnumerable<EventDescriptor<TEvent>> EventsSince(Address address, Version version, string streamId)
        {
            switch (version)
            {
                case ExistentVersion existentVersion:
                    int fromVersionInclusive = Convert.ToInt32(existentVersion.Value);
                    ReadStreamPage readStreamPage = await _streamStore.ReadStreamForwards(streamId, fromVersionInclusive, int.MaxValue, false);

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
                case NoneExistentVersion _:
                    break;
                default:
                    throw new NotSupportedException("Unknown type of existentVersion");
            }
        }
    }
}
