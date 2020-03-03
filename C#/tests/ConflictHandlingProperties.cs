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
            AppendEvents<InventoryItemEvent> appendEvents = (_, __, events) =>
            {
                eventStream.AddRange(events);
                return Task.FromResult(Ok<Version, SaveEventsError>(0L));
            };

            ResolveRemoteAddress resolveRemoteAddress = address => Task.FromResult(Ok<Uri, ResolveRemoteAddressError>(new Uri("")));
            Forward<InventoryItemCommand> forward = (_, __, ___) => Task.FromResult(Ok<Unit, ForwardError>(Unit.Instance));
            GetEventsSince<InventoryItemEvent> getEventsSince = (_, __) => AsyncEnumerable.Empty<EventDescriptor<InventoryItemEvent>>();
            FindConflicts<InventoryItemCommand, InventoryItemEvent> findConflicts = (_, __) => AsyncEnumerable.Empty<Conflict<InventoryItemCommand, InventoryItemEvent>>();
            OnConflictingCommandRejected<InventoryItemCommand, InventoryItemEvent> onConflictingCommandRejected = (conflicts) => Task.FromResult(Unit.Instance);

            var context = new BoundedContext<InventoryItemCommand, InventoryItemEvent>(
                new BoundedContextSettings<InventoryItemCommand, InventoryItemEvent>(
                    appendEvents,
                    getEventsSince,
                    resolveRemoteAddress,
                    forward,
                    findConflicts,
                    onConflictingCommandRejected,
                    garbageCollectionSettings), new CurrentThreadTaskScheduler());
            // for testing purposes make the aggregate block the current thread while processing
            var inventoryItem = await context.CreateAggregate<InventoryItem>();

            InventoryItemCommand command = new CreateInventoryItem("Product 1", true, 0);
            IVersion expectedVersion = new AnyVersion();
            await context.Send<InventoryItem>(new CommandDescriptor<InventoryItemCommand>(inventoryItem, command, expectedVersion));
            InventoryItemCommand command1 = new CheckInItemsToInventory(10);
            IVersion expectedVersion1 = new AnyVersion();
            await context.Send<InventoryItem>(new CommandDescriptor<InventoryItemCommand>(inventoryItem, command1, expectedVersion1));

            eventStream.Should().Equal(new List<InventoryItemEvent> {new InventoryItemCreated("Product 1", true, 0, inventoryItem), new ItemsCheckedInToInventory(10, inventoryItem) });

        }
 
               
        private static async IAsyncEnumerable<EventDescriptor<InventoryItemEvent>> GetEventsSince(Address address, IVersion version)
        {
            yield return new EventDescriptor<InventoryItemEvent>(
                new InventoryItemCreated("Product 1", true, 0, address), 1L);
            yield return new EventDescriptor<InventoryItemEvent>(new ItemsCheckedInToInventory(10, address),
                2L);
            yield return new EventDescriptor<InventoryItemEvent>(new InventoryItemRenamed("Product 2", address), 3L);
        }
        
        private static async IAsyncEnumerable<Conflict<InventoryItemCommand, InventoryItemEvent>> FindConflicts(InventoryItemCommand command, IAsyncEnumerable<EventDescriptor<InventoryItemEvent>> eventdescriptors)
        {
            var descriptor = await eventdescriptors.FirstAsync();
            var @event = descriptor.Event;
            yield return new Conflict<InventoryItemCommand, InventoryItemEvent>(command, @event, "Just another conflict");
        }
        
        [Fact(
            DisplayName = "Given there is a concurrency conflict and conflict resolution determines that there truly is a conflict, the last command should be rejected")]
        public async Task Property2()
        {
            var AppendedEvents = new List<InventoryItemEvent>();
            
            AppendEvents<InventoryItemEvent> appendEvents = (_, __, events) =>
            {
                AppendedEvents.AddRange(events);
                return Task.FromResult(Ok<Version, SaveEventsError>(1));
            };

            ResolveRemoteAddress resolveRemoteAddress = address => Task.FromResult(Ok<Uri, ResolveRemoteAddressError>(new Uri("")));
            Forward<InventoryItemCommand> forward = (_, __, ___) => Task.FromResult(Ok<Unit, ForwardError>(Unit.Instance));
            GetEventsSince<InventoryItemEvent> getEventsSince = GetEventsSince;
            FindConflicts<InventoryItemCommand, InventoryItemEvent> findConflicts = FindConflicts;

            var taskCompletionSource = new TaskCompletionSource<IAsyncEnumerable<Conflict<InventoryItemCommand, InventoryItemEvent>>>();
            OnConflictingCommandRejected<InventoryItemCommand, InventoryItemEvent> onConflictingCommandRejected = xs =>
            {
                taskCompletionSource.SetResult(xs);
                return Task.FromResult(Unit.Instance);
            };

            
            // for testing purposes make the aggregate block the current thread while processing
            var context = new BoundedContext<InventoryItemCommand, InventoryItemEvent>(
                new BoundedContextSettings<InventoryItemCommand, InventoryItemEvent>(
                    appendEvents,
                    getEventsSince,
                    resolveRemoteAddress,
                    forward,
                    findConflicts,
                    onConflictingCommandRejected,
                    garbageCollectionSettings), new CurrentThreadTaskScheduler());
            var inventoryItem = await context.CreateAggregate<InventoryItem>();
            

            Version expectedVersion = 2L;
            InventoryItemCommand inventoryItemCommand = new CheckInItemsToInventory(10);
            await context.Send<InventoryItem>(new CommandDescriptor<InventoryItemCommand>(inventoryItem, inventoryItemCommand, expectedVersion));

            AppendedEvents.Should().Equal(new List<InventoryItemEvent>(), "no event should be saved to the event store given the command was discarded");


            var conflicts = await taskCompletionSource.Task;
            conflicts.ToEnumerable().Should().Contain(conflict => conflict.Reason == "Just another conflict");
        }




        [Fact(
            DisplayName =
                "Given there is a concurrency conflict and conflict resolution determines that it is NO conflict and a technical concurrency exception arises when appending events to the stream, when retrying the expected event should be added to the stream")]
        public async Task Property3()
        {
            var calledBefore = false;
            var appendedEvents = new List<InventoryItemEvent>();
            var completionSource = new TaskCompletionSource<List<InventoryItemEvent>>();
            AppendEvents<InventoryItemEvent> appendEvents = (_, __, events) =>
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
            GetEventsSince<InventoryItemEvent> getEventsSince = GetEventsSince;

            FindConflicts<InventoryItemCommand, InventoryItemEvent> findConflicts = (command, eventDescriptors) => AsyncEnumerable.Empty<Conflict<InventoryItemCommand, InventoryItemEvent>>();
            OnConflictingCommandRejected<InventoryItemCommand, InventoryItemEvent> onConflictingCommandRejected = (conflicts) => Task.FromResult(Unit.Instance);

            var context = new BoundedContext<InventoryItemCommand, InventoryItemEvent>(
                new BoundedContextSettings<InventoryItemCommand, InventoryItemEvent>(
                    appendEvents,
                    getEventsSince,
                    resolveRemoteAddress,
                    forward,
                    findConflicts,
                    onConflictingCommandRejected,
                    garbageCollectionSettings), new CurrentThreadTaskScheduler());
            // for testing purposes make the aggregate block the current thread while processing
            var inventoryItem = await context.CreateAggregate<InventoryItem>();

            Version expectedVersion = 2L;
            InventoryItemCommand inventoryItemCommand = new CheckInItemsToInventory(10);
            await context.Send<InventoryItem>(new CommandDescriptor<InventoryItemCommand>(inventoryItem, inventoryItemCommand, expectedVersion));

            await completionSource.Task;

            appendedEvents.Should().Equal(new List<InventoryItemEvent> {new ItemsCheckedInToInventory(10, inventoryItem)});
        }

        [Fact(DisplayName = "Given there is a concurrency conflict and conflict resolution waves the conflict, the expected event should be added to the stream")]
        public async Task Property4()
        {
            var appendedEvents = new List<InventoryItemEvent>();
            var completionSource = new TaskCompletionSource<List<InventoryItemEvent>>();
            AppendEvents<InventoryItemEvent> appendEvents = (_, __, events) =>
            {
                appendedEvents.AddRange(events);
                completionSource.SetResult(appendedEvents);
                return Task.FromResult(Ok<Version, SaveEventsError>(0L));
            };

            // event stream is at version 3
            GetEventsSince<InventoryItemEvent> getEventsSince = GetEventsSince;
            ResolveRemoteAddress resolveRemoteAddress = address => Task.FromResult(Ok<Uri, ResolveRemoteAddressError>(new Uri("")));
            Forward<InventoryItemCommand> forward = (_, __, ___) => Task.FromResult(Ok<Unit, ForwardError>(Unit.Instance));
            FindConflicts<InventoryItemCommand, InventoryItemEvent> findConflicts = (command, eventDescriptors) =>
                AsyncEnumerable.Empty<Conflict<InventoryItemCommand, InventoryItemEvent>>();
            OnConflictingCommandRejected<InventoryItemCommand, InventoryItemEvent> onConflictingCommandRejected = (conflicts) => Task.FromResult(Unit.Instance);
            var context = new BoundedContext<InventoryItemCommand, InventoryItemEvent>(
                new BoundedContextSettings<InventoryItemCommand, InventoryItemEvent>(
                    appendEvents,
                    getEventsSince,
                    resolveRemoteAddress,
                    forward,
                    findConflicts,
                    onConflictingCommandRejected,
                    garbageCollectionSettings), new CurrentThreadTaskScheduler());
            // for testing purposes make the aggregate block the current thread while processing
            var inventoryItem = await context.CreateAggregate<InventoryItem>();

            // expected version is 2
            Version expectedVersion = 2L;

            InventoryItemCommand inventoryItemCommand = new CheckInItemsToInventory(10);
            await context.Send<InventoryItem>(new CommandDescriptor<InventoryItemCommand>(inventoryItem, inventoryItemCommand, expectedVersion));
            await completionSource.Task;

            appendedEvents.Should().Equal(new List<InventoryItemEvent> {new ItemsCheckedInToInventory(10, inventoryItem)});
        }

        [Fact(DisplayName = "Given there is no concurrency conflict, the expected event should be added to the stream")]
        public async Task Property5()
        {
            var appendedEvents = new List<InventoryItemEvent>();
            var completionSource = new TaskCompletionSource<List<InventoryItemEvent>>();
            AppendEvents<InventoryItemEvent> appendEvents = (_, __, events) =>
            {
                appendedEvents.AddRange(events);
                completionSource.SetResult(appendedEvents);
                return Task.FromResult(Ok<Version, SaveEventsError>(1));
            };
            GetEventsSince<InventoryItemEvent> getEventsSince = GetEventsSince;
            ResolveRemoteAddress resolveRemoteAddress = address => Task.FromResult(Ok<Uri, ResolveRemoteAddressError>(new Uri("")));
            Forward<InventoryItemCommand> forward = (_, __, ___) => Task.FromResult(Ok<Unit, ForwardError>(Unit.Instance));
            FindConflicts<InventoryItemCommand, InventoryItemEvent> findConflicts = (command, eventDescriptors) =>
                AsyncEnumerable.Empty<Conflict<InventoryItemCommand, InventoryItemEvent>>();
            OnConflictingCommandRejected<InventoryItemCommand, InventoryItemEvent> onConflictingCommandRejected = (conflicts) => Task.FromResult(Unit.Instance);

            var context = new BoundedContext<InventoryItemCommand, InventoryItemEvent>(
                new BoundedContextSettings<InventoryItemCommand, InventoryItemEvent>(
                    appendEvents,
                    getEventsSince,
                    resolveRemoteAddress,
                    forward,
                    findConflicts,
                    onConflictingCommandRejected,
                    garbageCollectionSettings), new CurrentThreadTaskScheduler());
            // for testing purposes make the aggregate block the current thread while processing
            var inventoryItem = await context.CreateAggregate<InventoryItem>();

            Version expectedVersion = 1L;

            InventoryItemCommand inventoryItemCommand = new CreateInventoryItem("Product 1", true, 0);
            await context.Send<InventoryItem>(new CommandDescriptor<InventoryItemCommand>(inventoryItem, inventoryItemCommand, expectedVersion));
            await completionSource.Task;

            appendedEvents.Should().Equal(new List<InventoryItemEvent> {new InventoryItemCreated("Product 1", true, 0, inventoryItem)});

        }
    }
}
