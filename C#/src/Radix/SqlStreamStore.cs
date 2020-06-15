using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Radix.Async;
using Radix.Option;
using Radix.Result;
using Radix.Validated;
using SqlStreamStore;
using SqlStreamStore.Streams;
using static Radix.Result.Extensions;

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
                        result = await _streamStore
                            .AppendToStream(eventStreamDescriptor.StreamIdentifier, ExpectedVersion.Any, newStreamMessages)
                            .Retry(Backoff.Exponentially());
                        return Ok<ExistingVersion, AppendEventsError>(result.CurrentVersion);
                    case NoneExistentVersion _:
                        await _streamStore.SetStreamMetadata(eventStreamDescriptor.StreamIdentifier, metadataJson: JsonSerializer.Serialize(eventStreamDescriptor));
                        expectedVersion = ExpectedVersion.NoStream;
                        result = await _streamStore
                            .AppendToStream(eventStreamDescriptor.StreamIdentifier, expectedVersion, newStreamMessages)
                            .Retry(Backoff.Exponentially());
                        return Ok<ExistingVersion, AppendEventsError>(result.CurrentVersion);
                    case ExistingVersion existentVersion:
                        expectedVersion = Convert.ToInt32(existentVersion.Value);
                        result = await _streamStore
                            .AppendToStream(eventStreamDescriptor.StreamIdentifier, expectedVersion, newStreamMessages)
                            .Retry(Backoff.Exponentially());
                        return Ok<ExistingVersion, AppendEventsError>(result.CurrentVersion);
                    default:
                        throw new NotSupportedException("Unknown type of existingVersion");
                }
            };

        public GetEventsSince<TEvent> CreateGetEventsSince<TEvent>(Func<Json, EventType, Option<TEvent>> parseEvent, Parse<EventMetaData, Json> parseMetaData)
        {

            async IAsyncEnumerable<EventDescriptor<TEvent>> CreateGetEventsSince(Address address, Version version, string streamId)
            {
                switch (version)
                {
                    case ExistingVersion existentVersion:
                        int fromVersionInclusive = Convert.ToInt32(existentVersion.Value);
                        ReadStreamPage readStreamPage = await _streamStore.ReadStreamForwards(streamId, fromVersionInclusive, int.MaxValue, false);

                        foreach (StreamMessage streamMessage in readStreamPage.Messages)
                        {
                                string jsonData = await streamMessage.GetJsonData();
                                EventType eventType = new EventType(streamMessage.Type);
                                Option<TEvent> optionalInventoryItemEvent = parseEvent(new Json(jsonData), eventType);
                                switch (optionalInventoryItemEvent)
                                {
                                    case None<TEvent> none:
                                        break;
                                    case Some<TEvent>(var @event):
                                        yield return new EventDescriptor<TEvent>(@event, existentVersion, eventType);
                                        break;
                                    default:
                                        throw new ArgumentOutOfRangeException(nameof(optionalInventoryItemEvent));
                                }
                                yield break;
                            
                        }

                        break;
                    case NoneExistentVersion _:
                        break;
                    default:
                        throw new NotSupportedException("Unknown type of version");
                }
            }

            return CreateGetEventsSince;
        }
    }
}
