using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Radix.Monoid;
using Radix.Option;
using Radix.Result;
using Radix.Tests.Models;
using Xunit;
using static Radix.Result.Extensions;
using static Radix.Option.Extensions;

namespace Radix.Tests
{


    public class ConflictHandlingProperties
    {
        private readonly TestSettings _testSettings = new TestSettings();

        [Fact(
            DisplayName =
                "Given an inventory item was created previously and we are disregarding concurrency conflicts, and items are checked into the inventory, the expected event should be added to the stream")]
        public async Task Property1()
        {
            AppendEvents<Json> appendEvents = (_, __, ___, events) => Task.FromResult(Ok<ExistingVersion, AppendEventsError>(0L));
            CheckForConflict<InventoryItemCommand, InventoryItemEvent> checkForConflict = (_, __) => None<Conflict<InventoryItemCommand, InventoryItemEvent>>();

            BoundedContext<InventoryItemCommand, InventoryItemEvent, Json> context = new BoundedContext<InventoryItemCommand, InventoryItemEvent, Json>(
                new BoundedContextSettings<InventoryItemCommand, InventoryItemEvent, Json>(
                    appendEvents,
                    _testSettings.GetEventsSince,
                    checkForConflict,
                    _testSettings.CollectionSettings,
                    _testSettings.Descriptor,
                    _testSettings.ToTransientEventDescriptor,
                    _testSettings.SerializeEvent,
                    _testSettings.SerializeMetaData
                ));
            // for testing purposes make the aggregate block the current thread while processing
            Aggregate<InventoryItemCommand, InventoryItemEvent> inventoryItem = context.Create(InventoryItem.Decide, InventoryItem.Update);

            Validated<InventoryItemCommand> checkIn =
                CheckInItemsToInventory
                    .Create(10);

            Result<InventoryItemEvent[], Error[]> result = await inventoryItem.Accept(checkIn);


            switch (result)
            {
                case Ok<InventoryItemEvent[], Error[]>(var events):
                    events.Should().Equal(
                        new List<InventoryItemEvent> {new ItemsCheckedInToInventory {Amount = 10}});
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
            AppendEvents<Json> appendEvents = (_, __, ___, events) => Task.FromResult(Ok<ExistingVersion, AppendEventsError>(1));
            CheckForConflict<InventoryItemCommand, InventoryItemEvent> checkForConflict = (command, descriptor) => Some(new Conflict<InventoryItemCommand, InventoryItemEvent>(command, null, "Just another conflict"));

            // for testing purposes make the aggregate block the current thread while processing
            BoundedContext<InventoryItemCommand, InventoryItemEvent, Json> context = new BoundedContext<InventoryItemCommand, InventoryItemEvent, Json>(
                new BoundedContextSettings<InventoryItemCommand, InventoryItemEvent, Json>(
                    appendEvents,
                    _testSettings.GetEventsSince,
                    checkForConflict,
                    _testSettings.CollectionSettings,
                    _testSettings.Descriptor,
                    _testSettings.ToTransientEventDescriptor,
                    _testSettings.SerializeEvent,
                    _testSettings.SerializeMetaData
                ));

            Aggregate<InventoryItemCommand, InventoryItemEvent> inventoryItem = context.Create(InventoryItem.Decide, InventoryItem.Update);
            Validated<InventoryItemCommand> checkin = CheckInItemsToInventory
                .Create(5);

            Result<InventoryItemEvent[], Error[]> result = await inventoryItem.Accept(checkin);

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
            AppendEvents<Json> appendEvents = (_, __, ___, events) =>
            {
                if (calledBefore)
                {
                    return Task.FromResult(Ok<ExistingVersion, AppendEventsError>(1L));
                }

                calledBefore = true;
                return Task.FromResult(Error<ExistingVersion, AppendEventsError>(new OptimisticConcurrencyError("Some conflict")));
            };
            CheckForConflict<InventoryItemCommand, InventoryItemEvent> checkForConflict = (command, eventDescriptors) =>
                None<Conflict<InventoryItemCommand, InventoryItemEvent>>();

            BoundedContext<InventoryItemCommand, InventoryItemEvent, Json> context = new BoundedContext<InventoryItemCommand, InventoryItemEvent, Json>(
                new BoundedContextSettings<InventoryItemCommand, InventoryItemEvent, Json>(
                    appendEvents,
                    _testSettings.GetEventsSince,
                    checkForConflict,
                    _testSettings.CollectionSettings,
                    _testSettings.Descriptor,
                    _testSettings.ToTransientEventDescriptor,
                    _testSettings.SerializeEvent,
                    _testSettings.SerializeMetaData
                ));

            Aggregate<InventoryItemCommand, InventoryItemEvent> inventoryItem = context.Create(InventoryItem.Decide, InventoryItem.Update);
            Validated<InventoryItemCommand> checkin = CheckInItemsToInventory.Create(10);

            Result<InventoryItemEvent[], Error[]> result = await inventoryItem.Accept(checkin);

            switch (result)
            {
                case Ok<InventoryItemEvent[], Error[]>(var events):
                    events.Should().BeEquivalentTo(new[] {new ItemsCheckedInToInventory {Amount = 10}}, "the event should be appended");
                    break;
                case Error<InventoryItemEvent[], Error[]>(var errors):
                    errors.Should().BeEmpty();
                    break;
            }
        }

        [Fact(DisplayName = "Given there is a concurrency conflict and conflict resolution waves the conflict, the expected event should be added to the stream")]
        public async Task Property4()
        {
            AppendEvents<Json> appendEvents = (_, __, ___, events) => Task.FromResult(Ok<ExistingVersion, AppendEventsError>(0L));

            // event stream is at existingVersion 3
            CheckForConflict<InventoryItemCommand, InventoryItemEvent> checkForConflict = (command, eventDescriptors) =>
                None<Conflict<InventoryItemCommand, InventoryItemEvent>>();

            BoundedContext<InventoryItemCommand, InventoryItemEvent, Json> context = new BoundedContext<InventoryItemCommand, InventoryItemEvent, Json>(
                new BoundedContextSettings<InventoryItemCommand, InventoryItemEvent, Json>(
                    appendEvents,
                    _testSettings.GetEventsSince,
                    checkForConflict,
                    _testSettings.CollectionSettings,
                    _testSettings.Descriptor,
                    _testSettings.ToTransientEventDescriptor,
                    _testSettings.SerializeEvent,
                    _testSettings.SerializeMetaData
                ));

            Aggregate<InventoryItemCommand, InventoryItemEvent> inventoryItem = context.Create(InventoryItem.Decide, InventoryItem.Update);
            Validated<InventoryItemCommand> checkin = CheckInItemsToInventory.Create(10);
            Result<InventoryItemEvent[], Error[]> result = await inventoryItem.Accept(checkin);

            switch (result)
            {
                case Ok<InventoryItemEvent[], Error[]>(var events):
                    events.Should().BeEquivalentTo(new[] {new ItemsCheckedInToInventory {Amount = 10}}, "the event should be appended");
                    break;
                case Error<InventoryItemEvent[], Error[]>(var errors):
                    errors.Should().BeEmpty();
                    break;
            }

        }

        [Fact(DisplayName = "Given there is no concurrency conflict, the expected event should be added to the stream")]
        public async Task Property5()
        {
            CheckForConflict<InventoryItemCommand, InventoryItemEvent> checkForConflict = (command, eventDescriptors) =>
                None<Conflict<InventoryItemCommand, InventoryItemEvent>>();

            BoundedContext<InventoryItemCommand, InventoryItemEvent, Json> context = new BoundedContext<InventoryItemCommand, InventoryItemEvent, Json>(
                new BoundedContextSettings<InventoryItemCommand, InventoryItemEvent, Json>(
                    (address, version, identifier, descriptors) => Task.FromResult(Ok<ExistingVersion, AppendEventsError>(0L)),
                    _testSettings.GetEventsSince,
                    checkForConflict,
                    _testSettings.CollectionSettings,
                    _testSettings.Descriptor,
                    _testSettings.ToTransientEventDescriptor,
                    _testSettings.SerializeEvent,
                    _testSettings.SerializeMetaData
                ));

            Aggregate<InventoryItemCommand, InventoryItemEvent> inventoryItem = context.Create(InventoryItem.Decide, InventoryItem.Update);

            Validated<InventoryItemCommand> create = CheckInItemsToInventory.Create(15);

            Result<InventoryItemEvent[], Error[]> result = await inventoryItem.Accept(create);
            switch (result)
            {
                case Ok<InventoryItemEvent[], Error[]>(var events):
                    events.Should().BeEquivalentTo(new ItemsCheckedInToInventory {Amount = 15});
                    break;
                case Error<InventoryItemEvent[], Error[]>(var errors):
                    errors.Should().BeEmpty();
                    break;
            }
        }
    }
}
