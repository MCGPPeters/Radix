﻿using Radix.Control.Task.Result;
using Radix.Data;
using Radix.Math.Pure.Logic.Order.Intervals;
using Version = Radix.Domain.Data.Version;

namespace Radix.Domain.Data;

[Alias<string>]
public partial record TenantId
{
    public static TenantId Default = (TenantId)"";
}

public interface EventStore<in TEventStore, in TEventStoreSettings>
    where TEventStore : EventStore<TEventStore, TEventStoreSettings>
{
    static abstract Task<Result<ExistingVersion, AppendEventsError>> AppendEvents<TEvent>(TEventStoreSettings eventStoreSettings, TenantId tenantId, Stream stream, Version expectedVersion,
        params TEvent[] events);

    static abstract IAsyncEnumerable<Event<TEvent>> GetEvents<TEvent>(TEventStoreSettings eventStoreSettings, TenantId tenantId, Stream stream, Closed<Version> interval);
}