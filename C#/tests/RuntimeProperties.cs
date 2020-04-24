using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Radix.Monoid;
using Radix.Result;
using Radix.Tests.Models;
using Xunit;
using static Radix.Result.Extensions;
using static Radix.Option.Extensions;

namespace Radix.Tests
{

    public class RuntimeProperties
    {
        private readonly GarbageCollectionSettings garbageCollectionSettings = new GarbageCollectionSettings
        {
            ScanInterval = TimeSpan.FromMilliseconds(500), IdleTimeout = TimeSpan.FromMilliseconds(500)
        };


        private static async IAsyncEnumerable<EventDescriptor<InventoryItemEvent>> GetEventsSince(Address address, IVersion version)
        {
            yield return new EventDescriptor<InventoryItemEvent>(
                address,
                new MessageId(Guid.NewGuid()),
                new MessageId(Guid.NewGuid()),
                new MessageId(Guid.NewGuid()),
                new InventoryItemCreated("Product 1", true, 0, address),
                1L);
            yield return new EventDescriptor<InventoryItemEvent>(
                address,
                new MessageId(Guid.NewGuid()),
                new MessageId(Guid.NewGuid()),
                new MessageId(Guid.NewGuid()),
                new ItemsCheckedInToInventory(19, address),
                2L);
            yield return new EventDescriptor<InventoryItemEvent>(
                address,
                new MessageId(Guid.NewGuid()),
                new MessageId(Guid.NewGuid()),
                new MessageId(Guid.NewGuid()),
                new InventoryItemRenamed("Product 2", address),
                3L);
        }

        [Fact(
            DisplayName =
                "Given an instance of an aggregate is not active, but it does exist, when sending a command it should be restored and process the command")]
        public async Task Test1()
        {
            List<InventoryItemEvent> appendedEvents = new List<InventoryItemEvent>();
            AppendEvents<InventoryItemEvent> appendEvents = (_, __, events) =>
            {
                appendedEvents.AddRange(events.Select(descriptor => descriptor.Event));
                return Task.FromResult(Ok<Version, AppendEventsError>(1));
            };

            GetEventsSince<InventoryItemEvent> getEventsSince = GetEventsSince;
            CheckForConflict<InventoryItemCommand, InventoryItemEvent> checkForConflict = (_, __) => None<Conflict<InventoryItemCommand, InventoryItemEvent>>();

            BoundedContext<InventoryItemCommand, InventoryItemEvent> context = new BoundedContext<InventoryItemCommand, InventoryItemEvent>(
                new BoundedContextSettings<InventoryItemCommand, InventoryItemEvent>(
                    new EventStoreStub(appendEvents, getEventsSince),
                    checkForConflict,
                    garbageCollectionSettings));
            // for testing purposes make the aggregate block the current thread while processing
            Address inventoryItem = await context.Create(InventoryItem.Update);
            await Task.Delay(TimeSpan.FromSeconds(1));

            Validated<InventoryItemCommand> removeItems = RemoveItemsFromInventory.Create(1);

            Result<InventoryItemEvent[], Error[]> result = await context.Send(inventoryItem, removeItems, InventoryItem.Update);
            switch (result)
            {
                case Ok<InventoryItemEvent[], Error[]>(var events):
                    events.Should().Equal(new List<InventoryItemEvent> {new ItemsRemovedFromInventory(1, inventoryItem)});
                    break;
                case Error<InventoryItemEvent[], Error[]>(var errors):
                    errors.Should().BeEmpty();
                    break;
            }

        }
    }
}
