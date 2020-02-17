using FluentAssertions;
using FsCheck;
using FsCheck.Xunit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Radix.Tests.Models;
using static Radix.Result.Extensions;

namespace Radix.Tests
{


    public class ConflictHandlingProperties
    {
        [Property(
            DisplayName =
                "Given an inventory item was created previously and we are disregarding concurrency conflicts, and items are checked into the inventory, the expected event should be added to the stream")]
        public void Property1(PositiveInt amount)
        {
            var eventStream = new List<InventoryItemEvent>();
            SaveEvents<InventoryItemEvent> saveEvents = (_, __, events) =>
            {
                eventStream.AddRange(events);
                return Task.FromResult(Ok<Unit, SaveEventsError>(Unit.Instance));
            };

            ResolveRemoteAddress resolveRemoteAddress = address => Task.FromResult(Ok<Uri, ResolveRemoteAddressError>(new Uri("")));
            Forward<InventoryItemCommand> forward = (_, __, ___) => Task.FromResult(Ok<Unit, ForwardError>(Unit.Instance));
            GetEventsSince<InventoryItemEvent> getEventsSince = (_, __) => Task.FromResult(new List<EventDescriptor<InventoryItemEvent>>());
            FindConflicts<InventoryItemCommand, InventoryItemEvent> findConflicts = (_, __) => Enumerable.Empty<Conflict<InventoryItemCommand, InventoryItemEvent>>();

            var context = new BoundedContext<InventoryItemCommand, InventoryItemEvent>(
                new BoundedContextSettings<InventoryItemCommand, InventoryItemEvent>(saveEvents, getEventsSince, resolveRemoteAddress, forward, findConflicts));
            // for testing purposes make the aggregate block the current thread while processing
            var inventoryItemSettings = new InventoryItemSettings(new TaskCompletionSource<IEnumerable<Conflict<InventoryItemCommand, InventoryItemEvent>>>());
            var inventoryItem = context.CreateAggregate<InventoryItem, InventoryItemSettings>(inventoryItemSettings, new CurrentThreadTaskScheduler());

            InventoryItemCommand command = new CreateInventoryItem("Product 1");
            IVersion expectedVersion = new AnyVersion();
            context.Send(new CommandDescriptor<InventoryItemCommand>(inventoryItem, command, expectedVersion));
            InventoryItemCommand command1 = new CheckInItemsToInventory(amount.Get);
            IVersion expectedVersion1 = new AnyVersion();
            context.Send(new CommandDescriptor<InventoryItemCommand>(inventoryItem, command1, expectedVersion1));

            eventStream.Should().Equal(new List<InventoryItemEvent> { new InventoryItemCreated("Product 1"), new ItemsCheckedInToInventory(amount.Get) });

        }

        [Property(
            DisplayName = "Given there is a concurrency conflict and conflict resolution determines that there truly is a conflict, the last command should be rejected")]
        public async void Property2(PositiveInt amount)
        {
            var AppendedEvents = new List<InventoryItemEvent>();
            SaveEvents<InventoryItemEvent> saveEvents = (_, __, events) =>
            {
                AppendedEvents.AddRange(events);
                return Task.FromResult(Ok<Unit, SaveEventsError>(Unit.Instance));
            };

            ResolveRemoteAddress resolveRemoteAddress = address => Task.FromResult(Ok<Uri, ResolveRemoteAddressError>(new Uri("")));
            Forward<InventoryItemCommand> forward = (_, __, ___) => Task.FromResult(Ok<Unit, ForwardError>(Unit.Instance));
            GetEventsSince<InventoryItemEvent> getEventsSince = (_, __) => Task.FromResult(
                new List<EventDescriptor<InventoryItemEvent>>
                {
                    new EventDescriptor<InventoryItemEvent>(new InventoryItemCreated("Product 1"), 1L),
                    new EventDescriptor<InventoryItemEvent>(new ItemsCheckedInToInventory(amount.Get), 2L),
                    new EventDescriptor<InventoryItemEvent>(new InventoryItemRenamed("Product 2"), 3L)
                });
            FindConflicts<InventoryItemCommand, InventoryItemEvent> findConflicts = (_, __) =>
                new List<Conflict<InventoryItemCommand, InventoryItemEvent>> { new Conflict<InventoryItemCommand, InventoryItemEvent>(null, null, "Just another conflict") };

            var context = new BoundedContext<InventoryItemCommand, InventoryItemEvent>(
                new BoundedContextSettings<InventoryItemCommand, InventoryItemEvent>(saveEvents, getEventsSince, resolveRemoteAddress, forward, findConflicts));
            // for testing purposes make the aggregate block the current thread while processing
            var taskCompletionSource = new TaskCompletionSource<IEnumerable<Conflict<InventoryItemCommand, InventoryItemEvent>>>();
            var inventoryItemSettings = new InventoryItemSettings(taskCompletionSource);
            var inventoryItem = context.CreateAggregate<InventoryItem, InventoryItemSettings>(inventoryItemSettings, new CurrentThreadTaskScheduler());

            Version expectedVersion = 2L;
            InventoryItemCommand inventoryItemCommand = new CheckInItemsToInventory(amount.Get);
            context.Send(new CommandDescriptor<InventoryItemCommand>(inventoryItem, inventoryItemCommand, expectedVersion));

            AppendedEvents.Should().Equal(new List<InventoryItemEvent>(), "no event should be saved to the event store given the command was discarded");


            var conflicts = await taskCompletionSource.Task;
            conflicts.Should().Contain(conflict => conflict.Reason == "Just another conflict");
        }

        [Property(
            DisplayName =
                "Given there is a concurrency conflict and conflict resolution determines that it is NO conflict and a technical concurrency exception arises when appending events to the stream, when retrying the expected event should be added to the stream")]
        public void Property3(PositiveInt amount)
        {
            var calledBefore = false;
            var appendedEvents = new List<InventoryItemEvent>();
            SaveEvents<InventoryItemEvent> saveEvents = (_, __, events) =>
            {
                if (calledBefore)
                {
                    appendedEvents.AddRange(events);
                    return Task.FromResult(Ok<Unit, SaveEventsError>(Unit.Instance));
                }

                calledBefore = true;
                return Task.FromResult(Error<Unit, SaveEventsError>(new OptimisticConcurrencyError()));
            };

            ResolveRemoteAddress resolveRemoteAddress = address => Task.FromResult(Ok<Uri, ResolveRemoteAddressError>(new Uri("")));
            Forward<InventoryItemCommand> forward = (_, __, ___) => Task.FromResult(Ok<Unit, ForwardError>(Unit.Instance));
            GetEventsSince<InventoryItemEvent> getEventsSince = (_, __) => Task.FromResult(
                new List<EventDescriptor<InventoryItemEvent>>
                {
                    new EventDescriptor<InventoryItemEvent>(new InventoryItemCreated("Product 1"), 1L),
                    new EventDescriptor<InventoryItemEvent>(new ItemsCheckedInToInventory(amount.Get), 2L),
                    new EventDescriptor<InventoryItemEvent>(new InventoryItemRenamed("Product 2"), 3L)
                });
            FindConflicts<InventoryItemCommand, InventoryItemEvent> findConflicts = (command, eventDescriptors) =>
                Enumerable.Empty<Conflict<InventoryItemCommand, InventoryItemEvent>>();

            var context = new BoundedContext<InventoryItemCommand, InventoryItemEvent>(
                new BoundedContextSettings<InventoryItemCommand, InventoryItemEvent>(saveEvents, getEventsSince, resolveRemoteAddress, forward, findConflicts));
            // for testing purposes make the aggregate block the current thread while processing
            var inventoryItemSettings = new InventoryItemSettings(new TaskCompletionSource<IEnumerable<Conflict<InventoryItemCommand, InventoryItemEvent>>>());
            var inventoryItem = context.CreateAggregate<InventoryItem, InventoryItemSettings>(inventoryItemSettings, new CurrentThreadTaskScheduler());

            Version expectedVersion = 2L;
            InventoryItemCommand inventoryItemCommand = new CheckInItemsToInventory(amount.Get);
            context.Send(new CommandDescriptor<InventoryItemCommand>(inventoryItem, inventoryItemCommand, expectedVersion));

            appendedEvents.Should().Equal(new List<InventoryItemEvent> { new ItemsCheckedInToInventory(amount.Get) });
        }

        [Property(DisplayName = "Given there is a concurrency conflict and conflict resolution waves the conflict, the expected event should be added to the stream")]
        public void Property4(PositiveInt amount)
        {
            var appendedEvents = new List<InventoryItemEvent>();
            SaveEvents<InventoryItemEvent> saveEvents = (_, __, events) =>
            {
                appendedEvents.AddRange(events);
                return Task.FromResult(Ok<Unit, SaveEventsError>(Unit.Instance));
            };

            // event stream is at version 3
            GetEventsSince<InventoryItemEvent> getEventsSince = (_, __) => Task.FromResult(
                new List<EventDescriptor<InventoryItemEvent>>
                {
                    new EventDescriptor<InventoryItemEvent>(new InventoryItemCreated("Product 1"), 1L),
                    new EventDescriptor<InventoryItemEvent>(new ItemsCheckedInToInventory(amount.Get), 2L),
                    new EventDescriptor<InventoryItemEvent>(new InventoryItemRenamed("Product 2"), 3L)
                });
            ResolveRemoteAddress resolveRemoteAddress = address => Task.FromResult(Ok<Uri, ResolveRemoteAddressError>(new Uri("")));
            Forward<InventoryItemCommand> forward = (_, __, ___) => Task.FromResult(Ok<Unit, ForwardError>(Unit.Instance));
            FindConflicts<InventoryItemCommand, InventoryItemEvent> findConflicts = (command, eventDescriptors) =>
                Enumerable.Empty<Conflict<InventoryItemCommand, InventoryItemEvent>>();

            var context = new BoundedContext<InventoryItemCommand, InventoryItemEvent>(
                new BoundedContextSettings<InventoryItemCommand, InventoryItemEvent>(saveEvents, getEventsSince, resolveRemoteAddress, forward, findConflicts));
            // for testing purposes make the aggregate block the current thread while processing
            var taskCompletionSource = new TaskCompletionSource<IEnumerable<Conflict<InventoryItemCommand, InventoryItemEvent>>>();
            var inventoryItemSettings = new InventoryItemSettings(taskCompletionSource);
            var inventoryItem = context.CreateAggregate<InventoryItem, InventoryItemSettings>(inventoryItemSettings, new CurrentThreadTaskScheduler());

            // expected version is 2
            Version expectedVersion = 2L;

            InventoryItemCommand inventoryItemCommand = new CheckInItemsToInventory(amount.Get);
            context.Send(new CommandDescriptor<InventoryItemCommand>(inventoryItem, inventoryItemCommand, expectedVersion));

            appendedEvents.Should().Equal(new List<InventoryItemEvent> { new ItemsCheckedInToInventory(amount.Get) });
        }

        [Property(DisplayName = "Given there is no concurrency conflict, the expected event should be added to the stream")]
        public void Property5(PositiveInt amount)
        {
            var appendedEvents = new List<InventoryItemEvent>();
            SaveEvents<InventoryItemEvent> saveEvents = (_, __, events) =>
            {
                appendedEvents.AddRange(events);
                return Task.FromResult(Ok<Unit, SaveEventsError>(Unit.Instance));
            };
            GetEventsSince<InventoryItemEvent> getEventsSince = (_, __) => Task.FromResult(
                new List<EventDescriptor<InventoryItemEvent>>
                {
                    new EventDescriptor<InventoryItemEvent>(new InventoryItemCreated("Product 1"), 1L),
                    new EventDescriptor<InventoryItemEvent>(new ItemsCheckedInToInventory(amount.Get), 2L),
                    new EventDescriptor<InventoryItemEvent>(new InventoryItemRenamed("Product 2"), 3L)
                });
            ResolveRemoteAddress resolveRemoteAddress = address => Task.FromResult(Ok<Uri, ResolveRemoteAddressError>(new Uri("")));
            Forward<InventoryItemCommand> forward = (_, __, ___) => Task.FromResult(Ok<Unit, ForwardError>(Unit.Instance));
            FindConflicts<InventoryItemCommand, InventoryItemEvent> findConflicts = (command, eventDescriptors) =>
                Enumerable.Empty<Conflict<InventoryItemCommand, InventoryItemEvent>>();

            var context = new BoundedContext<InventoryItemCommand, InventoryItemEvent>(
                new BoundedContextSettings<InventoryItemCommand, InventoryItemEvent>(saveEvents, getEventsSince, resolveRemoteAddress, forward, findConflicts));
            // for testing purposes make the aggregate block the current thread while processing
            var inventoryItemSettings = new InventoryItemSettings(new TaskCompletionSource<IEnumerable<Conflict<InventoryItemCommand, InventoryItemEvent>>>());
            var inventoryItem = context.CreateAggregate<InventoryItem, InventoryItemSettings>(inventoryItemSettings, new CurrentThreadTaskScheduler());

            Version expectedVersion = 1L;

            InventoryItemCommand inventoryItemCommand = new CreateInventoryItem("Product 1");
            context.Send(new CommandDescriptor<InventoryItemCommand>(inventoryItem, inventoryItemCommand, expectedVersion));

            appendedEvents.Should().Equal(new List<InventoryItemEvent> { new InventoryItemCreated("Product 1") });

        }
    }
}
