using Radix.Data;
using Radix.Math.Pure.Logic.Order.Intervals;

namespace Radix.Domain.Data;

/// <summary>
/// 
/// </summary>
//public static class EventStoreExtensions
//{
//    static Task<Result<ExistingVersion, AppendEventsError>> AppendEvents<TEventStore, TEvent>(
//        this TEventStore eventStore, string streamName, Version expectedVersion,
//        params TEvent[] events) where TEventStore : EventStore<TEventStore> =>
//        TEventStore.AppendEvents(eventStore, streamName, expectedVersion, events);

//    static IAsyncEnumerable<Event<TEvent>> GetEvents<TEvent, TEventStore, TEventStoreSettings>(TEventStore eventStore, string streamName,
//        Closed<Version> interval)
//        where TEventStore : EventStore<TEventStore, TEventStoreSettings> =>
//        TEventStore.GetEvents<TEvent>(eventStore, streamName, interval);
//}