using System.Threading.Tasks;

namespace Radix
{
    /// <summary>
    /// </summary>
    /// <param name="address">The address of the aggregate</param>
    /// <param name="expectedVersion">
    ///     The version the event stream is expected to be at when adding the new events. For the
    ///     purpose of optimistic concurrency
    /// </param>
    /// <param name="events">The events to append</param>
    /// <typeparam name="TEvent">The type of events</typeparam>
    /// <typeparam name="TError"></typeparam>
    /// <returns>
    ///     Either a next expected version of the event stream when the action succeeded or an error. The SaveEvents error is
    ///     on if the following:
    ///     - OptimisticConcurrencyError
    /// </returns>
    public delegate Task<Result<Version, AppendEventsError>> AppendEvents<in TEvent>(Address address, IVersion expectedVersion, TEvent[] events);
}
