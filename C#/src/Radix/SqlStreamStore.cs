using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Radix.Async;
using SqlStreamStore;
using SqlStreamStore.Streams;
using Extensions = Radix.Result.Extensions;

namespace Radix
{

    public class SqlStreamStore
    {

        private readonly IStreamStore _streamStore;

        public SqlStreamStore(IStreamStore streamStore) => _streamStore = streamStore;

        public AppendEvents<Json> AppendEvents =>
            async (address, version, streamIdentifier, events) =>
            {
                NewStreamMessage[] newStreamMessages = events.Select(
                    inventoryItemEvent => new NewStreamMessage(
                        inventoryItemEvent.MessageId.Value,
                        inventoryItemEvent.EventType.Value,
                        inventoryItemEvent.Event.Value,
                        inventoryItemEvent.EventMetaData.Value)).ToArray();

                Func<Task<AppendResult>> appendToStream;
                AppendResult result;
                int expectedVersion;

                switch (version)
                {
                    case AnyVersion _:
                        appendToStream = () => _streamStore.AppendToStream(streamIdentifier, ExpectedVersion.Any, newStreamMessages);
                        result = await appendToStream.Retry(Backoff.Exponentially());
                        return Extensions.Ok<ExistingVersion, AppendEventsError>(result.CurrentVersion);
                    case NoneExistentVersion _:
                        expectedVersion = ExpectedVersion.NoStream;
                        appendToStream = () => _streamStore.AppendToStream(streamIdentifier, expectedVersion, newStreamMessages);
                        result = await appendToStream.Retry(Backoff.Exponentially());
                        return Extensions.Ok<ExistingVersion, AppendEventsError>(result.CurrentVersion);
                    case ExistingVersion existentVersion:
                        expectedVersion = Convert.ToInt32(existentVersion.Value);
                        appendToStream = () => _streamStore.AppendToStream(streamIdentifier, expectedVersion, newStreamMessages);
                        result = await appendToStream.Retry(Backoff.Exponentially());
                        return Extensions.Ok<ExistingVersion, AppendEventsError>(result.CurrentVersion);
                    default:
                        throw new NotSupportedException("Unknown type of existingVersion");
                }
            };

        public GetEventsSince<Json> GetEventsSince => EventsSince;

        private async IAsyncEnumerable<EventDescriptor<Json>> EventsSince(Address address, Version version, string streamId)
        {
            switch (version)
            {
                case ExistingVersion existentVersion:
                    int fromVersionInclusive = Convert.ToInt32(existentVersion.Value);
                    ReadStreamPage readStreamPage = await _streamStore.ReadStreamForwards(streamId, fromVersionInclusive, int.MaxValue, false);

                    IEnumerable<Task<EventDescriptor<Json>>> eventDescriptors = readStreamPage.Messages.Select(
                        async streamMessage => new EventDescriptor<Json>(
                            new Address(streamMessage.MessageId),
                            new Json(streamMessage.JsonMetadata),
                            new Json(await streamMessage.GetJsonData()),
                            streamMessage.StreamVersion,
                            new EventType(streamMessage.Type)));

                    foreach (Task<EventDescriptor<Json>> eventDescriptor in eventDescriptors)
                    {
                        yield return await eventDescriptor;
                    }

                    break;
                case NoneExistentVersion _:
                    break;
                default:
                    throw new NotSupportedException("Unknown type of existingVersion");
            }
        }
    }
}
