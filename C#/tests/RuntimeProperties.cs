﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using FsCheck;
using FsCheck.Xunit;
using Radix.Tests.Models;
using Xunit;
using static Radix.Result.Extensions;

namespace Radix.Tests
{

    public class RuntimeProperties
    {
        private readonly GarbageCollectionSettings garbageCollectionSettings = new GarbageCollectionSettings
        {
            ScanInterval = TimeSpan.FromMilliseconds(500),
            IdleTimeout = TimeSpan.FromMilliseconds(500)
        };


        [Fact(
            DisplayName =
                "Given an instance of an aggregate is not active, but it does exist, when sending a command it should be restored and process the command")]
        public async Task Property2()
        {
            var appendedEvents = new List<InventoryItemEvent>();
            TaskCompletionSource<List<InventoryItemEvent>> completionSource = new TaskCompletionSource<List<InventoryItemEvent>>();
            SaveEvents<InventoryItemEvent> saveEvents = (_, __, events) =>
            {
                appendedEvents.AddRange(events);
                completionSource.SetResult(appendedEvents);
                return Task.FromResult(Ok<Version, SaveEventsError>(1));
            };
            ResolveRemoteAddress resolveRemoteAddress = address => Task.FromResult(Ok<Uri, ResolveRemoteAddressError>(new Uri("")));
            Forward<InventoryItemCommand> forward = (_, __, ___) => Task.FromResult(Ok<Unit, ForwardError>(Unit.Instance));
            GetEventsSince<InventoryItemEvent> getEventsSince = (_, __) =>
            {
                IEnumerable<EventDescriptor<InventoryItemEvent>> Results()
                {
                    yield return new EventDescriptor<InventoryItemEvent>(new InventoryItemCreated("Product 1"), 1L);
                    yield return new EventDescriptor<InventoryItemEvent>(new ItemsCheckedInToInventory(19), 2L);
                    yield return new EventDescriptor<InventoryItemEvent>(new InventoryItemRenamed("Product 2"), 3L);
                }

                return Task.FromResult(Results());
            };
            FindConflicts<InventoryItemCommand, InventoryItemEvent> findConflicts = (_, __) => Enumerable.Empty<Conflict<InventoryItemCommand, InventoryItemEvent>>();
            OnConflictingCommandRejected<InventoryItemCommand, InventoryItemEvent> onConflictingCommandRejected = (_) => Task.FromResult(Unit.Instance);

            var context = new BoundedContext<InventoryItemCommand, InventoryItemEvent>(
                new BoundedContextSettings<InventoryItemCommand, InventoryItemEvent>(
                    saveEvents,
                    getEventsSince,
                    resolveRemoteAddress,
                    forward,
                    findConflicts,
                    onConflictingCommandRejected,
                    garbageCollectionSettings), new CurrentThreadTaskScheduler());
            // for testing purposes make the aggregate block the current thread while processing
            var inventoryItem = context.CreateAggregate<InventoryItem>();
            await Task.Delay(TimeSpan.FromSeconds(1));

            await context.Send<InventoryItem>(new CommandDescriptor<InventoryItemCommand>(inventoryItem, new RemoveItemsFromInventory(1), new Version(3L)));
            await completionSource.Task;

            appendedEvents.Should().Equal(new List<InventoryItemEvent> { new ItemsRemovedFromInventory(1) });
        }
    }
}
