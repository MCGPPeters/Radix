﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Radix.Monoid;
using Radix.Result;
using Radix.Tests.Models;
using Radix.Validated;
using Xunit;
using static Radix.Result.Extensions;
using static Radix.Option.Extensions;

namespace Radix.Tests
{

    public class RuntimeProperties
    {
        private readonly GarbageCollectionSettings garbageCollectionSettings = new GarbageCollectionSettings
        {
            ScanInterval = TimeSpan.FromMilliseconds(500),
            IdleTimeout = TimeSpan.FromMilliseconds(500)
        };


        private static async IAsyncEnumerable<EventDescriptor<InventoryItemEvent>> GetEventsSince(Address address, IVersion version)
        {
            yield return new EventDescriptor<InventoryItemEvent>(
                new InventoryItemCreated("Product 1", true, 0, address),
                1L);
            yield return new EventDescriptor<InventoryItemEvent>(
                new ItemsCheckedInToInventory(19, address),
                2L);
            yield return new EventDescriptor<InventoryItemEvent>(new InventoryItemRenamed("Product 2", address), 3L);
        }

        [Fact(
            DisplayName =
                "Given an instance of an aggregate is not active, but it does exist, when sending a command it should be restored and process the command")]
        public async Task Test1()
        {
            var appendedEvents = new List<InventoryItemEvent>();
            AppendEvents<InventoryItemEvent> appendEvents = (_, __, events) =>
            {
                appendedEvents.AddRange(events);
                return Task.FromResult(Ok<Version, AppendEventsError>(1));
            };

            GetEventsSince<InventoryItemEvent> getEventsSince = GetEventsSince;
            CheckForConflict<InventoryItemCommand, InventoryItemEvent> checkForConflict = (_, __) => None<Conflict<InventoryItemCommand, InventoryItemEvent>>();

            var context = new BoundedContext<InventoryItemCommand, InventoryItemEvent>(
                new BoundedContextSettings<InventoryItemCommand, InventoryItemEvent>(
                    new EventStoreStub(appendEvents, getEventsSince),
                    checkForConflict,
                    garbageCollectionSettings));
            // for testing purposes make the aggregate block the current thread while processing
            var inventoryItem = await context.Create<InventoryItem>();
            await Task.Delay(TimeSpan.FromSeconds(1));

            var removeItems = RemoveItemsFromInventory.Create(1);
            var command = Command<InventoryItemCommand>.Create(() => removeItems);

            switch (command)
            {
                case Valid<Command<InventoryItemCommand>> validCommand:
                    IVersion expectedVersion = new AnyVersion();
                    var result = await context.Send<InventoryItem>(new CommandDescriptor<InventoryItemCommand>(inventoryItem, validCommand, new Version(3L)));
                    switch (result)
                    {
                        case Ok<InventoryItemEvent[], string[]>(var events):
                            events.Should().Equal(new List<InventoryItemEvent> {new ItemsRemovedFromInventory(1, inventoryItem)});
                            break;
                        case Error<InventoryItemEvent[], string[]>(var errors):
                            errors.Should().BeEmpty();
                            break;
                    }

                    break;
            }
        }
    }
}
