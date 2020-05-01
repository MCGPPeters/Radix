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


    public class ConflictHandlingProperties
    {
        private readonly GarbageCollectionSettings garbageCollectionSettings = new GarbageCollectionSettings
        {
            ScanInterval = TimeSpan.FromMinutes(1), IdleTimeout = TimeSpan.FromMinutes(60)
        };


        private static async IAsyncEnumerable<EventDescriptor<InventoryItemEvent>> GetEventsSince(Address address, Version version, string streamIdentifier)
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
                new ItemsCheckedInToInventory(10, address),
                2L);
            yield return new EventDescriptor<InventoryItemEvent>(
                address,
                new MessageId(Guid.NewGuid()),
                new MessageId(Guid.NewGuid()),
                new MessageId(Guid.NewGuid()),
                new InventoryItemRenamed("Product 2", address),
                3L);
        }

        private static Option<Conflict<InventoryItemCommand, InventoryItemEvent>> FindConflict(InventoryItemCommand command, EventDescriptor<InventoryItemEvent> descriptor)
        {
            InventoryItemEvent @event = descriptor.Event;
            return Some(new Conflict<InventoryItemCommand, InventoryItemEvent>(command, @event, "Just another conflict"));
        }

        [Fact(
            DisplayName =
                "Given an inventory item was created previously and we are disregarding concurrency conflicts, and items are checked into the inventory, the expected event should be added to the stream")]
        public async Task Property1()
        {
            AppendEvents<InventoryItemEvent> appendEvents = (_, __, ___, events) => Task.FromResult(Ok<ExistentVersion, AppendEventsError>(0L));
            GetEventsSince<InventoryItemEvent> getEventsSince = GetEventsSince;
            CheckForConflict<InventoryItemCommand, InventoryItemEvent> checkForConflict = (_, __) => None<Conflict<InventoryItemCommand, InventoryItemEvent>>();

            BoundedContext<InventoryItemCommand, InventoryItemEvent> context = new BoundedContext<InventoryItemCommand, InventoryItemEvent>(
                new BoundedContextSettings<InventoryItemCommand, InventoryItemEvent>(appendEvents, getEventsSince,
                    checkForConflict,
                    garbageCollectionSettings));
            // for testing purposes make the aggregate block the current thread while processing
            Aggregate<InventoryItemCommand, InventoryItemEvent> inventoryItem = await context.Create(InventoryItem.Decide, InventoryItem.Update);

            Validated<InventoryItemCommand> checkin =
                CheckInItemsToInventory
                    .Create(10);

            Result<InventoryItemEvent[], Error[]> result = await inventoryItem.Send(checkin);


            switch (result)
            {
                case Ok<InventoryItemEvent[], Error[]>(var events):
                    events.Should().Equal(
                        new List<InventoryItemEvent> {new ItemsCheckedInToInventory(10, inventoryItem.Address)});
                    break;
                case Error<InventoryItemEvent[], Error[]>(var errors):
                    errors.Should().BeEmpty();
                    break;
            }
        }

        [Fact(
            DisplayName = "Given there is a concurrency conflict and conflict resolution determines that there truly is a conflict, the last command should be rejected")]
        public async Task Property2()
        {
            AppendEvents<InventoryItemEvent> appendEvents = (_, __, ___, events) => Task.FromResult(Ok<ExistentVersion, AppendEventsError>(1));
            GetEventsSince<InventoryItemEvent> getEventsSince = GetEventsSince;
            CheckForConflict<InventoryItemCommand, InventoryItemEvent> checkForConflict = FindConflict;

            // for testing purposes make the aggregate block the current thread while processing
            BoundedContext<InventoryItemCommand, InventoryItemEvent> context = new BoundedContext<InventoryItemCommand, InventoryItemEvent>(
                new BoundedContextSettings<InventoryItemCommand, InventoryItemEvent>(
                    appendEvents, getEventsSince,
                    checkForConflict,
                    garbageCollectionSettings));

            Aggregate<InventoryItemCommand, InventoryItemEvent> inventoryItem = await context.Create(InventoryItem.Decide, InventoryItem.Update);
            Validated<InventoryItemCommand> checkin = CheckInItemsToInventory
                .Create(10);

            Result<InventoryItemEvent[], Error[]> result = await inventoryItem.Send(checkin);

            switch (result)
            {
                case Ok<InventoryItemEvent[], Error[]>(var events):
                    events.Should().Equal(new List<InventoryItemEvent>(), "no event should be saved to the event store given the command was discarded");
                    break;
                case Error<InventoryItemEvent[], Error[]>(var errors):
                    errors.Should().Contain(error => error.Message == "Just another conflict");
                    break;
            }

        }


        [Fact(
            DisplayName =
                "Given there is a concurrency conflict and conflict resolution determines that it is NO conflict and a technical concurrency exception arises when appending events to the stream, when retrying the expected event should be added to the stream")]
        public async Task Property3()
        {
            bool calledBefore = false;
            List<InventoryItemEvent> appendedEvents = new List<InventoryItemEvent>();
            AppendEvents<InventoryItemEvent> appendEvents = (_, __, ___, events) =>
            {
                if (calledBefore)
                {
                    appendedEvents.AddRange(events.Select(descriptor => descriptor.Event));
                    return Task.FromResult(Ok<ExistentVersion, AppendEventsError>(1));
                }

                calledBefore = true;
                return Task.FromResult(Error<ExistentVersion, AppendEventsError>(new OptimisticConcurrencyError("Some conflict")));
            };

            GetEventsSince<InventoryItemEvent> getEventsSince = GetEventsSince;
            CheckForConflict<InventoryItemCommand, InventoryItemEvent> checkForConflict = (command, eventDescriptors) => None<Conflict<InventoryItemCommand, InventoryItemEvent>>();

            BoundedContext<InventoryItemCommand, InventoryItemEvent> context = new BoundedContext<InventoryItemCommand, InventoryItemEvent>(
                new BoundedContextSettings<InventoryItemCommand, InventoryItemEvent>(
                    appendEvents, getEventsSince,
                    checkForConflict,
                    garbageCollectionSettings));

            Aggregate<InventoryItemCommand, InventoryItemEvent> inventoryItem = await context.Create(InventoryItem.Decide, InventoryItem.Update);
            Validated<InventoryItemCommand> checkin = CheckInItemsToInventory.Create(10);

            Result<InventoryItemEvent[], Error[]> result = await inventoryItem.Send(checkin);

            switch (result)
            {
                case Ok<InventoryItemEvent[], Error[]>(var events):
                    events.Should().BeEquivalentTo(new[] {new ItemsCheckedInToInventory(10, inventoryItem.Address)}, "the event should be appended");
                    break;
                case Error<InventoryItemEvent[], Error[]>(var errors):
                    errors.Should().BeEmpty();
                    break;
            }
        }

        [Fact(DisplayName = "Given there is a concurrency conflict and conflict resolution waves the conflict, the expected event should be added to the stream")]
        public async Task Property4()
        {
            List<InventoryItemEvent> appendedEvents = new List<InventoryItemEvent>();
            AppendEvents<InventoryItemEvent> appendEvents = (_, __, ___, events) =>
            {
                appendedEvents.AddRange(events.Select(descriptor => descriptor.Event));
                return Task.FromResult(Ok<ExistentVersion, AppendEventsError>(0L));
            };

            // event stream is at existentVersion 3
            GetEventsSince<InventoryItemEvent> getEventsSince = GetEventsSince;
            CheckForConflict<InventoryItemCommand, InventoryItemEvent> checkForConflict = (command, eventDescriptors) =>
                None<Conflict<InventoryItemCommand, InventoryItemEvent>>();

            BoundedContext<InventoryItemCommand, InventoryItemEvent> context = new BoundedContext<InventoryItemCommand, InventoryItemEvent>(
                new BoundedContextSettings<InventoryItemCommand, InventoryItemEvent>(
                    appendEvents, getEventsSince,
                    checkForConflict,
                    garbageCollectionSettings));

            Aggregate<InventoryItemCommand, InventoryItemEvent> inventoryItem = await context.Create(InventoryItem.Decide, InventoryItem.Update);
            Validated<InventoryItemCommand> checkin = CheckInItemsToInventory.Create(10);
            Result<InventoryItemEvent[], Error[]> result = await inventoryItem.Send(checkin);

            switch (result)
            {
                case Ok<InventoryItemEvent[], Error[]>(var events):
                    events.Should().BeEquivalentTo(new[] {new ItemsCheckedInToInventory(10, inventoryItem.Address)}, "the event should be appended");
                    break;
                case Error<InventoryItemEvent[], Error[]>(var errors):
                    errors.Should().BeEmpty();
                    break;
            }

        }

        [Fact(DisplayName = "Given there is no concurrency conflict, the expected event should be added to the stream")]
        public async Task Property5()
        {
            List<InventoryItemEvent> appendedEvents = new List<InventoryItemEvent>();
            AppendEvents<InventoryItemEvent> appendEvents = (_, __, ___, events) =>
            {
                appendedEvents.AddRange(events.Select(descriptor => descriptor.Event));
                return Task.FromResult(Ok<ExistentVersion, AppendEventsError>(1));
            };
            GetEventsSince<InventoryItemEvent> getEventsSince = GetEventsSince;
            CheckForConflict<InventoryItemCommand, InventoryItemEvent> checkForConflict = (command, eventDescriptors) =>
                None<Conflict<InventoryItemCommand, InventoryItemEvent>>();

            BoundedContext<InventoryItemCommand, InventoryItemEvent> context = new BoundedContext<InventoryItemCommand, InventoryItemEvent>(
                new BoundedContextSettings<InventoryItemCommand, InventoryItemEvent>(
                    appendEvents, getEventsSince,
                    checkForConflict,
                    garbageCollectionSettings));

            Aggregate<InventoryItemCommand, InventoryItemEvent> inventoryItem = await context.Create(InventoryItem.Decide, InventoryItem.Update);

            Validated<InventoryItemCommand> create = CheckInItemsToInventory.Create(15);

            Result<InventoryItemEvent[], Error[]> result = await inventoryItem.Send(create);
            switch (result)
            {
                case Ok<InventoryItemEvent[], Error[]>(var events):
                    events.Should().BeEquivalentTo(new ItemsCheckedInToInventory(15, inventoryItem.Address));
                    break;
                case Error<InventoryItemEvent[], Error[]>(var errors):
                    errors.Should().BeEmpty();
                    break;
            }
        }
    }
}
