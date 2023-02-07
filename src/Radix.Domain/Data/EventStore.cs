﻿using Radix.Control.Task.Result;
using Radix.Data;
using Radix.Math.Pure.Logic.Order.Intervals;
using Version = Radix.Domain.Data.Version;

namespace Radix.Domain.Data;

public interface EventStore<TEventStore>
    where TEventStore : EventStore<TEventStore>
{
    static abstract Task<Result<ExistingVersion, AppendEventsError>> AppendEvents<TEvent>(string streamName, Version expectedVersion,
        params TEvent[] events);

    static abstract IAsyncEnumerable<Event<TEvent>> GetEvents<TEvent>(string streamName, Closed<Version> interval);
}