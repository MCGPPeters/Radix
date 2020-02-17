using System.Collections.Generic;
using System.Threading.Tasks;

namespace Radix
{
    public interface AggregateSettings<TCommand, TEvent>
    {
        /// <summary>
        ///     Called when a true concurrency conflict according to business rules had occured.
        ///     No events have been recorded that would have been generated as a consequence of
        ///     the command if it would have succeeded
        /// </summary>
        /// <param name="conflicts"></param>
        /// <returns>Unit</returns>
        Task<Unit> OnConflictingCommandRejected(IEnumerable<Conflict<TCommand, TEvent>> conflicts);
    }
}
