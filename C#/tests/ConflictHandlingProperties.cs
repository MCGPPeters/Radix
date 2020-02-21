using System;
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


    public class ConflictHandlingProperties
    {
        private readonly GarbageCollectionSettings garbageCollectionSettings = new GarbageCollectionSettings
        {
            ScanInterval = TimeSpan.FromMinutes(1),
            IdleTimeout = TimeSpan.FromMinutes(60)
        };

        [Fact(
            DisplayName =
                "Given an inventory item was created previously and we are disregarding concurrency conflicts, and items are checked into the inventory, the expected event should be added to the stream")]
        public async Task Property1()
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
            OnConflictingCommandRejected<InventoryItemCommand, InventoryItemEvent> onConflictingCommandRejected = (conflicts) => Task.FromResult(Unit.Instance);

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

            InventoryItemCommand command = new CreateInventoryItem("Product 1");
            IVersion expectedVersion = new AnyVersion();
            await context.Send<InventoryItem>(new CommandDescriptor<InventoryItemCommand>(inventoryItem, command, expectedVersion));
            InventoryItemCommand command1 = new CheckInItemsToInventory(10);
            IVersion expectedVersion1 = new AnyVersion();
            await context.Send<InventoryItem>(new CommandDescriptor<InventoryItemCommand>(inventoryItem, command1, expectedVersion1));

            eventStream.Should().Equal(new List<InventoryItemEvent> {new InventoryItemCreated("Product 1"), new ItemsCheckedInToInventory(10)});

        }

        [Fact(
            DisplayName = "Given there is a concurrency conflict and conflict resolution determines that there truly is a conflict, the last command should be rejected")]
        public async Task Property2()
        {
            var AppendedEvents = new List<InventoryItemEvent>();
            SaveEvents<InventoryItemEvent> saveEvents = (_, __, events) =>
            {
                AppendedEvents.AddRange(events);
                return Task.FromResult(Ok<Version, SaveEventsError>(1));
            };

            ResolveRemoteAddress resolveRemoteAddress = address => Task.FromResult(Ok<Uri, ResolveRemoteAddressError>(new Uri("")));
            Forward<InventoryItemCommand> forward = (_, __, ___) => Task.FromResult(Ok<Unit, ForwardError>(Unit.Instance));
            GetEventsSince<InventoryItemEvent> getEventsSince = (_, __) => Task.FromResult(
                new List<EventDescriptor<InventoryItemEvent>>
                {
                    new EventDescriptor<InventoryItemEvent>(new InventoryItemCreated("Product 1"), 1L),
                    new EventDescriptor<InventoryItemEvent>(new ItemsCheckedInToInventory(10), 2L),
                    new EventDescriptor<InventoryItemEvent>(new InventoryItemRenamed("Product 2"), 3L)
                }.AsEnumerable());
            FindConflicts<InventoryItemCommand, InventoryItemEvent> findConflicts = (_, __) =>
                new List<Conflict<InventoryItemCommand, InventoryItemEvent>> {new Conflict<InventoryItemCommand, InventoryItemEvent>(null, null, "Just another conflict")};

            var taskCompletionSource = new TaskCompletionSource<IEnumerable<Conflict<InventoryItemCommand, InventoryItemEvent>>>();
            OnConflictingCommandRejected<InventoryItemCommand, InventoryItemEvent> onConflictingCommandRejected = (conflicts) =>
            {
                taskCompletionSource.SetResult(conflicts);
                return Task.FromResult(Unit.Instance);
            };

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

            Version expectedVersion = 2L;
            InventoryItemCommand inventoryItemCommand = new CheckInItemsToInventory(10);
            await context.Send<InventoryItem>(new CommandDescriptor<InventoryItemCommand>(inventoryItem, inventoryItemCommand, expectedVersion));

            AppendedEvents.Should().Equal(new List<InventoryItemEvent>(), "no event should be saved to the event store given the command was discarded");


            var conflicts = await taskCompletionSource.Task;
            conflicts.Should().Contain(conflict => conflict.Reason == "Just another conflict");
        }

        [Fact(
            DisplayName =
                "Given there is a concurrency conflict and conflict resolution determines that it is NO conflict and a technical concurrency exception arises when appending events to the stream, when retrying the expected event should be added to the stream")]
        public async Task Property3()
        {
            var calledBefore = false;
            var appendedEvents = new List<InventoryItemEvent>();
            var completionSource = new TaskCompletionSource<List<InventoryItemEvent>>();
            SaveEvents<InventoryItemEvent> saveEvents = (_, __, events) =>
            {
                if (calledBefore)
                {
                    appendedEvents.AddRange(events);
                    completionSource.SetResult(appendedEvents);
                    return Task.FromResult(Ok<Version, SaveEventsError>(1));
                }

                calledBefore = true;
                completionSource.SetResult(new List<InventoryItemEvent>());
                return Task.FromResult(Error<Version, SaveEventsError>(new OptimisticConcurrencyError()));
            };

            ResolveRemoteAddress resolveRemoteAddress = address => Task.FromResult(Ok<Uri, ResolveRemoteAddressError>(new Uri("")));
            Forward<InventoryItemCommand> forward = (_, __, ___) => Task.FromResult(Ok<Unit, ForwardError>(Unit.Instance));
            GetEventsSince<InventoryItemEvent> getEventsSince = (_, __) => Task.FromResult(
                new List<EventDescriptor<InventoryItemEvent>>
                {
                    new EventDescriptor<InventoryItemEvent>(new InventoryItemCreated("Product 1"), 1L),
                    new EventDescriptor<InventoryItemEvent>(new ItemsCheckedInToInventory(10), 2L),
                    new EventDescriptor<InventoryItemEvent>(new InventoryItemRenamed("Product 2"), 3L)
                }.AsEnumerable());

            FindConflicts<InventoryItemCommand, InventoryItemEvent> findConflicts = (command, eventDescriptors) => Enumerable.Empty<Conflict<InventoryItemCommand, InventoryItemEvent>>();
            OnConflictingCommandRejected<InventoryItemCommand, InventoryItemEvent> onConflictingCommandRejected = (conflicts) => Task.FromResult(Unit.Instance);

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

            Version expectedVersion = 2L;
            InventoryItemCommand inventoryItemCommand = new CheckInItemsToInventory(10);
            await context.Send<InventoryItem>(new CommandDescriptor<InventoryItemCommand>(inventoryItem, inventoryItemCommand, expectedVersion));

            await completionSource.Task;

            appendedEvents.Should().Equal(new List<InventoryItemEvent> {new ItemsCheckedInToInventory(10)});
        }

        [Fact(DisplayName = "Given there is a concurrency conflict and conflict resolution waves the conflict, the expected event should be added to the stream")]
        public async Task Property4()
        {
            var appendedEvents = new List<InventoryItemEvent>();
            var completionSource = new TaskCompletionSource<List<InventoryItemEvent>>();
            SaveEvents<InventoryItemEvent> saveEvents = (_, __, events) =>
            {
                appendedEvents.AddRange(events);
                completionSource.SetResult(appendedEvents);
                return Task.FromResult(Ok<Version, SaveEventsError>(0L));
            };

            // event stream is at version 3
            GetEventsSince<InventoryItemEvent> getEventsSince = (_, __) => Task.FromResult(
                new List<EventDescriptor<InventoryItemEvent>>
                {
                    new EventDescriptor<InventoryItemEvent>(new InventoryItemCreated("Product 1"), 1L),
                    new EventDescriptor<InventoryItemEvent>(new ItemsCheckedInToInventory(10), 2L),
                    new EventDescriptor<InventoryItemEvent>(new InventoryItemRenamed("Product 2"), 3L)
                }.AsEnumerable());
            ResolveRemoteAddress resolveRemoteAddress = address => Task.FromResult(Ok<Uri, ResolveRemoteAddressError>(new Uri("")));
            Forward<InventoryItemCommand> forward = (_, __, ___) => Task.FromResult(Ok<Unit, ForwardError>(Unit.Instance));
            FindConflicts<InventoryItemCommand, InventoryItemEvent> findConflicts = (command, eventDescriptors) =>
                Enumerable.Empty<Conflict<InventoryItemCommand, InventoryItemEvent>>();
            OnConflictingCommandRejected<InventoryItemCommand, InventoryItemEvent> onConflictingCommandRejected = (conflicts) => Task.FromResult(Unit.Instance);
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

            // expected version is 2
            Version expectedVersion = 2L;

            InventoryItemCommand inventoryItemCommand = new CheckInItemsToInventory(10);
            await context.Send<InventoryItem>(new CommandDescriptor<InventoryItemCommand>(inventoryItem, inventoryItemCommand, expectedVersion));
            await completionSource.Task;

            appendedEvents.Should().Equal(new List<InventoryItemEvent> {new ItemsCheckedInToInventory(10)});
        }

        [Fact(DisplayName = "Given there is no concurrency conflict, the expected event should be added to the stream")]
        public async Task Property5()
        {
            var appendedEvents = new List<InventoryItemEvent>();
            var completionSource = new TaskCompletionSource<List<InventoryItemEvent>>();
            SaveEvents<InventoryItemEvent> saveEvents = (_, __, events) =>
            {
                appendedEvents.AddRange(events);
                completionSource.SetResult(appendedEvents);
                return Task.FromResult(Ok<Version, SaveEventsError>(1));
            };
            GetEventsSince<InventoryItemEvent> getEventsSince = (_, __) =>
            {
                IEnumerable<EventDescriptor<InventoryItemEvent>> Results()
                {
                    yield return new EventDescriptor<InventoryItemEvent>(new InventoryItemCreated("Product 1"), 1L);
                    yield return new EventDescriptor<InventoryItemEvent>(new ItemsCheckedInToInventory(10), 2L);
                    yield return new EventDescriptor<InventoryItemEvent>(new InventoryItemRenamed("Product 2"), 3L);
                }

                return Task.FromResult(Results());
            };
            ResolveRemoteAddress resolveRemoteAddress = address => Task.FromResult(Ok<Uri, ResolveRemoteAddressError>(new Uri("")));
            Forward<InventoryItemCommand> forward = (_, __, ___) => Task.FromResult(Ok<Unit, ForwardError>(Unit.Instance));
            FindConflicts<InventoryItemCommand, InventoryItemEvent> findConflicts = (command, eventDescriptors) =>
                Enumerable.Empty<Conflict<InventoryItemCommand, InventoryItemEvent>>();
            OnConflictingCommandRejected<InventoryItemCommand, InventoryItemEvent> onConflictingCommandRejected = (conflicts) => Task.FromResult(Unit.Instance);

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

            Version expectedVersion = 1L;

            InventoryItemCommand inventoryItemCommand = new CreateInventoryItem("Product 1");
            await context.Send<InventoryItem>(new CommandDescriptor<InventoryItemCommand>(inventoryItem, inventoryItemCommand, expectedVersion));
            await completionSource.Task;

            appendedEvents.Should().Equal(new List<InventoryItemEvent> {new InventoryItemCreated("Product 1")});

        }
    }
}
