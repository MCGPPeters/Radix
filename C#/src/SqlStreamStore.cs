﻿ using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Radix.Async;
using SqlStreamStore;
using SqlStreamStore.Streams;
using static Radix.Option.Extensions;
using Extensions = Radix.Result.Extensions;

namespace Radix
{

    public class SqlStreamStore<TEvent> : EventStore<TEvent> where TEvent : Event
    {

        private static readonly IStreamStore streamStore = new InMemoryStreamStore();

        public AppendEvents<TEvent> AppendEvents
        {
            get
            {
                return async (address, version, events) =>
                {
                    var newStreamMessages = events.Select(
                        inventoryItemEvent =>
                        {
                            var jsonMessage = JsonSerializer.Serialize(inventoryItemEvent);
                            var messageId = inventoryItemEvent.Address.Value;
                            return new NewStreamMessage(messageId, inventoryItemEvent.ToString(), jsonMessage);
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
            }
        }

        public GetEventsSince<TEvent> GetEventsSince => EventsSince;

        private async IAsyncEnumerable<EventDescriptor<TEvent>> EventsSince(Address address, IVersion version)
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
                        var @event = JsonSerializer.Deserialize<TEvent>(await streamMessage.GetJsonData());
                        return new EventDescriptor<TEvent>(@event, streamMessage.StreamVersion);
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
    }
}
