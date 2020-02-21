using System.Collections.Generic;
using System.Threading.Tasks;

namespace Radix
{
    /// <summary>
    ///     Called when a true concurrency conflict according to business rules had occured.
    ///     No events have been recorded that would have been generated as a consequence of
    ///     the command if it would have succeeded
    /// </summary>
    /// <param name="conflicts"></param>
    /// <returns>Unit</returns>
    public delegate Task<Unit> OnConflictingCommandRejected<TCommand, TEvent>(IEnumerable<Conflict<TCommand, TEvent>> conflicts);
}
