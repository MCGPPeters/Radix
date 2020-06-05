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

    public class SqlStreamStore
    {

        private readonly IStreamStore _streamStore;

        public SqlStreamStore(IStreamStore streamStore) => _streamStore = streamStore;

        public AppendEvents<Json> AppendEvents =>
            async (address, version, eventStreamDescriptor, events) =>
            {
                NewStreamMessage[] newStreamMessages = events.Select(
                    eventDescriptor => new NewStreamMessage(
                        eventDescriptor.MessageId.Value,
                        eventDescriptor.EventType.Value,
                        eventDescriptor.Event.Value,
                        eventDescriptor.EventMetaData.Value)).ToArray();

                Func<Task<AppendResult>> appendToStream;
                AppendResult result;
                int expectedVersion;

                switch (version)
                {
                    case AnyVersion _:
                        appendToStream = () => _streamStore.AppendToStream(eventStreamDescriptor.StreamIdentifier, ExpectedVersion.Any, newStreamMessages);
                        result = await appendToStream.Retry(Backoff.Exponentially());
                        return Extensions.Ok<ExistingVersion, AppendEventsError>(result.CurrentVersion);
                    case NoneExistentVersion _:
                        await _streamStore.SetStreamMetadata(eventStreamDescriptor.StreamIdentifier, metadataJson: JsonSerializer.Serialize(eventStreamDescriptor));
                        expectedVersion = ExpectedVersion.NoStream;
                        appendToStream = () => _streamStore.AppendToStream(eventStreamDescriptor.StreamIdentifier, expectedVersion, newStreamMessages);
                        result = await appendToStream.Retry(Backoff.Exponentially());
                        return Extensions.Ok<ExistingVersion, AppendEventsError>(result.CurrentVersion);
                    case ExistingVersion existentVersion:
                        expectedVersion = Convert.ToInt32(existentVersion.Value);
                        appendToStream = () => _streamStore.AppendToStream(eventStreamDescriptor.StreamIdentifier, expectedVersion, newStreamMessages);
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
                    StreamMetadataResult streamMetadata = await _streamStore.GetStreamMetadata(streamId);
                    EventStreamDescriptor streamDescriptor = JsonSerializer.Deserialize<EventStreamDescriptor>(streamMetadata.MetadataJson);
                    ReadStreamPage readStreamPage = await _streamStore.ReadStreamForwards(streamId, fromVersionInclusive, int.MaxValue, false);

                    IEnumerable<Task<EventDescriptor<Json>>> eventDescriptors = readStreamPage.Messages.Select(
                        async streamMessage => new EventDescriptor<Json>(
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
                    throw new NotSupportedException("Unknown type of version");
            }
        }
    }
}
