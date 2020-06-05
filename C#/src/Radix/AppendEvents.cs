using System.Threading.Tasks;

namespace Radix
{
    /// <summary>
    /// </summary>
    /// <param name="address">The address of the aggregate</param>
    /// <param name="expectedVersion">
    ///     The existingVersion the event stream is expected to be at when adding the new events. For the
    ///     purpose of optimistic concurrency
    /// </param>
    /// <param name="events">The events to append</param>
    /// <typeparam name="TFormat">The format of events</typeparam>
    /// <returns>
    ///     Either a next expected existingVersion of the event stream when the action succeeded or an error. The SaveEvents
    ///     error is
    ///     on if the following:
    ///     - OptimisticConcurrencyError
    /// </returns>
    public delegate Task<Result<ExistingVersion, AppendEventsError>> AppendEvents<TFormat>(Address address, Version expectedVersion, EventStreamDescriptor eventStreamDescriptor,
        TransientEventDescriptor<TFormat>[] transientEventDescriptors);
}
