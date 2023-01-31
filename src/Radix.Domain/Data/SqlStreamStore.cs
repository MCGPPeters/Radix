using Radix.Control.Nullable;
using System.Text.Json;
using Radix.Control.Task;
using Radix.Data;
using SqlStreamStore;
using SqlStreamStore.Streams;
using static Radix.Control.Result.Extensions;
using System;
using static Radix.Control.Option.Extensions;

namespace Radix.Domain.Data;

public class SqlStreamStore
{

    private readonly IStreamStore _streamStore;

    public SqlStreamStore(IStreamStore streamStore) => _streamStore = streamStore;

    public AppendEvents<Json> AppendEvents =>
        async (id, version, eventStreamDescriptor, events) =>
        {
            NewStreamMessage[] newStreamMessages = events.Select(
                eventDescriptor => new NewStreamMessage(
                    eventDescriptor.MessageId.Value,
                    eventDescriptor.EventType.Value,
                    eventDescriptor.Event.Value,
                    eventDescriptor.EventMetaData.Value)).ToArray();

            AppendResult result;
            int expectedVersion;

            // the object cast is a workarround for https://github.com/dotnet/csharplang/issues/1229
            switch ((object)version)
            {
                case AnyVersion _:
                    result = await _streamStore
                        .AppendToStream(eventStreamDescriptor.StreamIdentifier, ExpectedVersion.Any, newStreamMessages)
                        .Retry(Backoff.Exponentially());
                    return Ok<ExistingVersion, AppendEventsError>(new ExistingVersion(result.CurrentVersion));
                case MinimumVersion _:
                    await _streamStore.SetStreamMetadata(eventStreamDescriptor.StreamIdentifier, metadataJson: JsonSerializer.Serialize(eventStreamDescriptor));
                    expectedVersion = ExpectedVersion.NoStream;
                    result = await _streamStore
                        .AppendToStream(eventStreamDescriptor.StreamIdentifier, expectedVersion, newStreamMessages)
                        .Retry(Backoff.Exponentially());
                    return Ok<ExistingVersion, AppendEventsError>(new ExistingVersion(result.CurrentVersion));
                case ExistingVersion existentVersion:
                    expectedVersion = Convert.ToInt32(existentVersion.Value);
                    result = await _streamStore
                        .AppendToStream(eventStreamDescriptor.StreamIdentifier, expectedVersion, newStreamMessages)
                        .Retry(Backoff.Exponentially());
                    return Ok<ExistingVersion, AppendEventsError>(new ExistingVersion(result.CurrentVersion));
                default:
                    throw new NotSupportedException("Unknown type of existingVersion");
            }
        };

    public static Option<TEvent> ParseEvent<TEvent>(Json json, EventType eventType)
    {
        if (string.Equals(eventType.Value, nameof(TEvent), StringComparison.Ordinal))
        {
            var counterIncremented = JsonSerializer.Deserialize<TEvent>(json.Value);
            return counterIncremented.AsOption();
        }


        return None<TEvent>();
    }


    public GetEventsSince<TEvent> CreateGetEventsSince<TEvent>()
        where TEvent : notnull
    {

        async IAsyncEnumerable<EventDescriptor<TEvent>> CreateGetEventsSince(Aggregate.Id id, Version version, string streamId)
        {
            // the object cast is a workaround for https://github.com/dotnet/csharplang/issues/1229
            switch ((object)version)
            {
                case ExistingVersion existentVersion:
                    int fromVersionInclusive = Convert.ToInt32(existentVersion.Value);
                    ReadStreamPage readStreamPage = await _streamStore.ReadStreamForwards(streamId, fromVersionInclusive, int.MaxValue, false);

                    foreach (StreamMessage streamMessage in readStreamPage.Messages)
                    {
                        string jsonData = await streamMessage.GetJsonData();
                        Validated<EventType> eventType = EventType.Create(streamMessage.Type);
                        switch (eventType)
                        {
                            case Valid<EventType>(var et):
                                Option<TEvent> optionalInventoryItemEvent = ParseEvent<TEvent>(new Json(jsonData), et);
                                switch (optionalInventoryItemEvent)
                                {
                                    case None<TEvent> _:
                                        break;
                                    case Some<TEvent>(var @event):
                                        yield return new EventDescriptor<TEvent>(@event, existentVersion, et);
                                        break;
                                    default:
                                        throw new ArgumentOutOfRangeException(nameof(optionalInventoryItemEvent));
                                }

                                break;
                        }

                        yield break;

                    }

                    break;
                case MinimumVersion _:
                    break;
                default:
                    throw new NotSupportedException("Unknown type of version");
            }
        }

        return CreateGetEventsSince;
    }
}
