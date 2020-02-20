using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Radix.Tests
{
    /// <summary>
    ///     Executes a task on the current thread
    /// </summary>
    public class CurrentThreadTaskScheduler : TaskScheduler
    {

        /// <summary>
        ///     <inheritdoc />
        /// </summary>
        public override int MaximumConcurrencyLevel => 1;

        /// <summary>
        ///     <inheritdoc />
        /// </summary>
        protected override void QueueTask(Task task)
        {
            TryExecuteTask(task);
        }

        /// <summary>
        ///     <inheritdoc />
        /// </summary>
        protected override bool TryExecuteTaskInline(
            Task task,
            bool taskWasPreviouslyQueued)
        {
            return TryExecuteTask(task);
        }

        /// <summary>
        ///     <inheritdoc />
        /// </summary>
        protected override IEnumerable<Task> GetScheduledTasks()
        {
            return Enumerable.Empty<Task>();
        }
    }
}
