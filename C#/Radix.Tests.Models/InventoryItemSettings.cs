using System.Collections.Generic;
using System.Threading.Tasks;

namespace Radix.Tests.Models
{
    /// <summary>
    ///     The setting class is needed for signaling the found actual concurrency conflicts
    /// </summary>
    public class InventoryItemSettings : AggregateSettings<InventoryItemCommand, InventoryItemEvent>
    {
        private readonly TaskCompletionSource<IEnumerable<Conflict<InventoryItemCommand, InventoryItemEvent>>> _taskCompletionSource;

        public InventoryItemSettings(TaskCompletionSource<IEnumerable<Conflict<InventoryItemCommand, InventoryItemEvent>>> taskCompletionSource)
        {
            _taskCompletionSource = taskCompletionSource;
        }

        /// <summary>
        ///     Signals the conflicts that were passed on by the runtime
        /// </summary>
        /// <param name="conflicts"></param>
        /// <returns></returns>
        public Task<Unit> OnConflictingCommandRejected(IEnumerable<Conflict<InventoryItemCommand, InventoryItemEvent>> conflicts)
        {
            _taskCompletionSource.SetResult(conflicts);
            return Task.FromResult(Unit.Instance);
        }
    }
}
