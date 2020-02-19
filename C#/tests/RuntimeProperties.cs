using FsCheck.Xunit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Radix.Tests.Models;
using static Radix.Result.Extensions;

namespace Radix.Tests
{

    public class RuntimeProperties
    {
        [Property(
            DisplayName =
                "Given an instance of an aggregate is not active, but it does exist, when sending a command it should be restored and process the command")]
        public void Property1()
        {
            var eventStream = new List<InventoryItemEvent>();
            SaveEvents<InventoryItemEvent> saveEvents = (_, __, events) =>
            {
                eventStream.AddRange(events);
                return Task.FromResult(Ok<Version, SaveEventsError>(0L));
            };

            ResolveRemoteAddress resolveRemoteAddress = address => Task.FromResult(Ok<Uri, ResolveRemoteAddressError>(new Uri("")));
            Forward<InventoryItemCommand> forward = (_, __, ___) => Task.FromResult(Ok<Unit, ForwardError>(Unit.Instance));
            GetEventsSince<InventoryItemEvent> getEventsSince = (_, __) => Task.FromResult(Enumerable.Empty<EventDescriptor<InventoryItemEvent>>());
            FindConflicts<InventoryItemCommand, InventoryItemEvent> findConflicts = (_, __) => Enumerable.Empty<Conflict<InventoryItemCommand, InventoryItemEvent>>();
            OnConflictingCommandRejected<InventoryItemCommand, InventoryItemEvent> onConflictingCommandRejected = (conflicts, taskCompletionSource) =>
            {
                taskCompletionSource.SetResult(conflicts);
                return Task.FromResult(Unit.Instance);
            };

            var context = new BoundedContext<InventoryItemCommand, InventoryItemEvent>(
                new BoundedContextSettings<InventoryItemCommand, InventoryItemEvent>(saveEvents, getEventsSince, resolveRemoteAddress, forward, findConflicts, onConflictingCommandRejected));
            // for testing purposes make the aggregate block the current thread while processing
            var inventoryItemSettings = new InventoryItemSettings(new TaskCompletionSource<IEnumerable<Conflict<InventoryItemCommand, InventoryItemEvent>>>());
            var inventoryItem = context.CreateAggregate<InventoryItem>(new CurrentThreadTaskScheduler());
        }
    }
}
