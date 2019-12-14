using FluentAssertions;
using FsCheck;
using FsCheck.Xunit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Radix.Result.Extensions;

namespace Radix.Tests
{
    public interface InventoryItemEvent
    {
    }

    public class InventoryItemDeactivated : InventoryItemEvent
    {
    }

    public class InventoryItemCreated : InventoryItemEvent
    {

        public InventoryItemCreated(string name)
        {
            Name = name;

        }

        public string Name { get; }

        protected bool Equals(InventoryItemCreated other)
        {
            return string.Equals(Name, other.Name);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((InventoryItemCreated)obj);
        }

        public override int GetHashCode()
        {
            return Name != null ? Name.GetHashCode() : 0;
        }
    }

    public class InventoryItemRenamed : InventoryItemEvent
    {

        public InventoryItemRenamed(string name)
        {
            Name = name;

        }

        public string Name { get; }
    }

    public class ItemsCheckedInToInventory : InventoryItemEvent
    {

        public ItemsCheckedInToInventory(int amount)
        {
            Amount = amount;

        }

        public int Amount { get; }

        protected bool Equals(ItemsCheckedInToInventory other)
        {
            return Amount == other.Amount;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((ItemsCheckedInToInventory)obj);
        }

        public override int GetHashCode()
        {
            return Amount;
        }
    }

    public class ItemsRemovedFromInventory : InventoryItemEvent
    {

        public ItemsRemovedFromInventory(int amount)
        {
            Amount = amount;

        }

        public int Amount { get; }
    }

    public interface InventoryItemCommand
    {
    }

    public struct DeactivateInventoryItem : InventoryItemCommand
    {
    }


    public class CreateInventoryItem : InventoryItemCommand
    {

        public CreateInventoryItem(string name)
        {
            Name = name;
        }

        public string Name { get; }
    }

    public class RenameInventoryItem : InventoryItemCommand
    {

        public RenameInventoryItem(string name)
        {
            Name = name;
        }

        public string Name { get; }
    }

    public class CheckInItemsToInventory : InventoryItemCommand
    {

        public CheckInItemsToInventory(int amount)
        {
            Amount = amount;
        }

        public int Amount { get; }
    }

    public class RemoveItemsFromInventory : InventoryItemCommand
    {

        public RemoveItemsFromInventory(int amount)
        {
            Amount = amount;
        }

        public int Amount { get; }
    }


    /// <summary>
    ///     The error to return when saving events causes an optimistic concurrency error
    /// </summary>
    internal class OptimisticConcurrencyError : SaveEventsError
    {
        public SaveEventsError Append(SaveEventsError x, SaveEventsError y)
        {
            throw new NotImplementedException();
        }

        public SaveEventsError Empty()
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    ///     An error while save to the event store
    /// </summary>
    public interface SaveEventsError : Monoid<SaveEventsError>
    {
    }

    public interface ResolveRemoteAddressError : Monoid<ResolveRemoteAddressError>
    {
    }

    public interface ForwardError : Monoid<ForwardError>
    {
    }


    /// <summary>
    /// The setting class is needed for signaling the found actual concurrency conflicts
    /// </summary>
    public class InventoryItemSettings : AggregateSettings<InventoryItemCommand, InventoryItemEvent>
    {
        private readonly TaskCompletionSource<IEnumerable<Conflict<InventoryItemCommand, InventoryItemEvent>>> _taskCompletionSource;

        public InventoryItemSettings(TaskCompletionSource<IEnumerable<Conflict<InventoryItemCommand, InventoryItemEvent>>> taskCompletionSource)
        {
            _taskCompletionSource = taskCompletionSource;
        }

        /// <summary>
        /// Signals the conflicts that were passed on by the runtime
        /// </summary>
        /// <param name="conflicts"></param>
        /// <returns></returns>
        public Task<Unit> OnConflictingCommandRejected(IEnumerable<Conflict<InventoryItemCommand, InventoryItemEvent>> conflicts)
        {
            _taskCompletionSource.SetResult(conflicts);
            return Task.FromResult(Unit.Instance);
        }
    }


    /// <summary>
    /// </summary>
    /// <param name="address">The address of the aggregate</param>
    /// <param name="expectedVersion">
    ///     The version the event stream is expected to be at when adding the new events.true For the
    ///     purpose of optimistic concurrency
    /// </param>
    /// <param name="events"></param>
    /// <typeparam name="TEvent">The type of events</typeparam>
    /// <returns>
    ///     Either a confirmation the action succeeded (in the for of an instance of Unit) or an error. The SaveEvents error is
    ///     on if the following:
    ///     - OptimisticConcurrencyError
    /// </returns>
    public delegate Task<Result<Unit, SaveEventsError>> SaveEvents<in TEvent>(Address address, IVersion expectedVersion, IEnumerable<TEvent> events);

    /// <summary>
    ///     Get all event descriptors for an aggregate since (excluding) the supplied version
    /// </summary>
    /// <param name="address">Address of the aggregate</param>
    /// <param name="version"></param>
    /// <typeparam name="TEvent"></typeparam>
    /// <returns></returns>
    public delegate Task<List<EventDescriptor<TEvent>>> GetEventsSince<TEvent>(Address address, IVersion version);

    /// <summary>
    ///     Returns the uri of the resource hosting the aggregate with the indicated address
    ///     todo : will shift to the multiaddress spec https://github.com/multiformats/multiaddr for addressing
    /// </summary>
    /// <param name="address"></param>
    /// <returns>Either the Uri or an error</returns>
    public delegate Task<Result<Uri, ResolveRemoteAddressError>> ResolveRemoteAddress(Address address);

    /// <summary>
    ///     Forwards a command to an other context
    /// </summary>
    /// <param name="command"></param>
    /// <param name="destination"></param>
    /// <param name="address"></param>
    /// <typeparam name="TCommand"></typeparam>
    /// <returns></returns>
    public delegate Task<Result<Unit, ForwardError>> Forward<in TCommand>(TCommand command, Uri destination, Address address);

    /// <summary>
    ///     Returns the first conflict between the comment and an event, if any
    /// </summary>
    /// <param name="command"></param>
    /// <typeparam name="TCommand"></typeparam>
    /// <typeparam name="TEvent"></typeparam>
    /// <returns></returns>
    public delegate IEnumerable<Conflict<TCommand, TEvent>> FindConflicts<TCommand, TEvent>(TCommand command, IEnumerable<EventDescriptor<TEvent>> eventDescriptors);

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
            var inventoryItem = context.CreateAggregate<InventoryItem, InventoryItemSettings>(new CurrentThreadTaskScheduler(), inventoryItemSettings);

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
            var inventoryItem = context.CreateAggregate<InventoryItem, InventoryItemSettings>(new CurrentThreadTaskScheduler(), inventoryItemSettings);

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
            var inventoryItem = context.CreateAggregate<InventoryItem, InventoryItemSettings>(new CurrentThreadTaskScheduler(), inventoryItemSettings);

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
            var inventoryItem = context.CreateAggregate<InventoryItem, InventoryItemSettings>(new CurrentThreadTaskScheduler(), inventoryItemSettings);

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
            var inventoryItem = context.CreateAggregate<InventoryItem, InventoryItemSettings>(new CurrentThreadTaskScheduler(), inventoryItemSettings);

            Version expectedVersion = 1L;

            InventoryItemCommand inventoryItemCommand = new CreateInventoryItem("Product 1");
            context.Send(new CommandDescriptor<InventoryItemCommand>(inventoryItem, inventoryItemCommand, expectedVersion));

            appendedEvents.Should().Equal(new List<InventoryItemEvent> { new InventoryItemCreated("Product 1") });

        }
    }
}
