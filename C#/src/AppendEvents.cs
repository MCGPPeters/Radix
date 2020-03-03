using System.Collections.Generic;
using System.Threading.Tasks;

namespace Radix
{
    /// <summary>
    /// </summary>
    /// <param name="address">The address of the aggregate</param>
    /// <param name="expectedVersion">
    ///     The version the event stream is expected to be at when adding the new events.true For the
    ///     purpose of optimistic concurrency
    /// </param>
    /// <param name="events"></param>
    /// <typeparam name="TEvent">The type of events</typeparam>
    /// <returns>
    ///     Either a next expected version of the event stream when the action succeeded or an error. The SaveEvents error is
    ///     on if the following:
    ///     - OptimisticConcurrencyError
    /// </returns>
    public delegate Task<Result<Version, SaveEventsError>> AppendEvents<in TEvent>(Address address, IVersion expectedVersion, IEnumerable<TEvent> events);
}
