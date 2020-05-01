using System.Threading.Tasks;

namespace Radix
{
    /// <summary>
    /// </summary>
    /// <param name="address">The address of the aggregate</param>
    /// <param name="expectedVersion">
    ///     The existentVersion the event stream is expected to be at when adding the new events. For the
    ///     purpose of optimistic concurrency
    /// </param>
    /// <param name="events">The events to append</param>
    /// <typeparam name="TEvent">The type of events</typeparam>
    /// <typeparam name="TError"></typeparam>
    /// <typeparam name="TState"></typeparam>
    /// <returns>
    ///     Either a next expected existentVersion of the event stream when the action succeeded or an error. The SaveEvents error is
    ///     on if the following:
    ///     - OptimisticConcurrencyError
    /// </returns>
    public delegate Task<Result<ExistentVersion, AppendEventsError>> AppendEvents<TEvent>(Address address, Version expectedVersion, string streamIdentifier,
        TransientEventDescriptor<TEvent>[] transientEventDescriptors);
}
