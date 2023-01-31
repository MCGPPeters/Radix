namespace Radix.Domain.Data;

/// <summary>
///     Get all events for an aggregate since (excluding) the supplied existingVersion
/// </summary>
/// <param name="id">State of the aggregate</param>
/// <param name="version"></param>
/// <param name="streamId"></param>
/// <typeparam name="TEvent"></typeparam>
/// <returns></returns>
public delegate IAsyncEnumerable<EventDescriptor<TEvent>> GetEventsSince<TEvent>(Aggregate.Id id, Version version, string streamId);
