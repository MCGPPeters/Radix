using Radix.Data;
using Radix.Domain.Data;
using Radix.Math.Pure.Logic.Order.Intervals;
using Stream = Radix.Domain.Data.Stream;
using Version = Radix.Domain.Data.Version;

namespace Radix.Domain.EventStore;

public delegate Task<Result<ExistingVersion, AppendEventsError>> AppendEvents<in TEvent>(TenantId tenantId,
    Stream stream, Version expectedVersion,
    params TEvent[] events);

public delegate IAsyncEnumerable<Event<TEvent>> GetEvents<TEvent>(TenantId tenantId, Stream stream,
    Closed<Version> interval);

