﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static Radix.Tests.Result.Extensions;
using Radix.Tests.Result;
using FsCheck.Xunit;
using FsCheck;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using FluentAssertions;
using Radix.Tests.Maybe;

namespace Radix.Tests
{
    public interface IVersion
    {

    }

    public struct Version : IVersion
    {
        private long Value { get; }

        private Version(long value)
        {
            Value = value;;
        }

        public static implicit operator Version(long value)
        {
            return new Version(value);
        }

        public static implicit operator long(Version version)
        {
            return version.Value;
        }
    }

    public struct AnyVersion : IVersion
    {

    }

    public struct Address
    {
        private Guid _guid;

        public Address(Guid guid)
        {
            _guid = guid;
        }
    }

    /// <summary>
    /// The interface an Aggregate root must conform to
    /// </summary>
    /// <typeparam name="TState">The type of the aggregate root. 
    /// The new() contraint is intended to enforce the ability to create an aggregate root with its initial values. Its initial state.</typeparam>
    /// <typeparam name="TEvent">The type of events the aggregate root generates</typeparam>
    /// <typeparam name="TCommand">The type of commands the aggregate root accepts</typeparam>
    public interface Aggregate<out TState, TEvent, in TCommand> where TState : new()
    {
        List<TEvent> Decide(TCommand command);

        TState Apply(TEvent @event);
    }

    public class InventoryItem : Aggregate<InventoryItem, InventoryItemEvent, InventoryItemCommand>
    {
        public string Name { get; }
        public bool Activated { get; }
        public int Count { get; }

        public InventoryItem()
        {
            Name = "";
            Activated = true;
            Count = 0;
        }

        public InventoryItem(string name, bool activated, int count)
        {
            Name = name;
            Activated = activated;
            Count = count;
        }

        public List<InventoryItemEvent> Decide(InventoryItemCommand command)
        {
            return command switch
            {
                DeactivateInventoryItem _ => new List<InventoryItemEvent> {new InventoryItemDeactivated()},
                CreateInventoryItem createInventoryItem => new List<InventoryItemEvent> {new InventoryItemCreated(createInventoryItem.Name)},
                RenameInventoryItem renameInventoryItem => new List<InventoryItemEvent> {new InventoryItemRenamed(renameInventoryItem.Name)},
                CheckInItemsToInventory checkInItemsToInventory => new List<InventoryItemEvent> {new ItemsCheckedInToInventory(checkInItemsToInventory.Amount)},
                RemoveItemsFromInventory removeItemsFromInventory => new List<InventoryItemEvent> {new ItemsRemovedFromInventory(removeItemsFromInventory.Amount)},
                _ => throw new NotSupportedException("Unknown command")
            };
        }

        public InventoryItem Apply(InventoryItemEvent @event)
        {
            return @event switch
            {
                InventoryItemCreated inventoryItemCreated => new InventoryItem(inventoryItemCreated.Name, Activated, Count),
                InventoryItemDeactivated _ => new InventoryItem(Name, false, Count),
                ItemsCheckedInToInventory itemsCheckedInToInventory => new InventoryItem(Name, Activated, Count + itemsCheckedInToInventory.Amount),
                ItemsRemovedFromInventory itemsRemovedFromInventory => new InventoryItem(Name, Activated, Count - itemsRemovedFromInventory.Amount),
                InventoryItemRenamed inventoryItemRenamed => new InventoryItem(inventoryItemRenamed.Name, Activated, Count),
                _ => throw new NotSupportedException("Unknown event")
            };
        }
    }

    public interface InventoryItemEvent
    {
    }

    public struct InventoryItemDeactivated : InventoryItemEvent { }

    public struct InventoryItemCreated : InventoryItemEvent
    {
        public string Name { get; }

        public InventoryItemCreated(string name)
        {
            Name = name;

        }
    }

    public struct InventoryItemRenamed : InventoryItemEvent
    {
        public string Name { get; }

        public InventoryItemRenamed(string name)
        {
            Name = name;

        }
    }

    public struct ItemsCheckedInToInventory : InventoryItemEvent
    {
        public int Amount { get; }

        public ItemsCheckedInToInventory(int amount)
        {
            Amount = amount;

        }
    }

    public struct ItemsRemovedFromInventory : InventoryItemEvent
    {
        public int Amount { get; }

        public ItemsRemovedFromInventory(int amount)
        {
            Amount = amount;

        }
    }

    public interface InventoryItemCommand
    {
    }

    public struct DeactivateInventoryItem : InventoryItemCommand {}


    public struct CreateInventoryItem : InventoryItemCommand
    {
        public string Name { get; }

        public CreateInventoryItem(string name)
        {
            Name = name;
        }
    }

    public struct RenameInventoryItem : InventoryItemCommand
    {
        public string Name { get; }

        public RenameInventoryItem(string name)
        {
            Name = name;
        }
    }

    public struct CheckInItemsToInventory : InventoryItemCommand
    {
        public int Amount { get; }

        public CheckInItemsToInventory(int amount)
        {
            Amount = amount;
        }
    }

    public struct RemoveItemsFromInventory : InventoryItemCommand
    {
        public int Amount { get; }

        public RemoveItemsFromInventory(int amount)
        {
            Amount = amount;
        }
    }

    public interface ContextCommand{}

    public struct IssueCommand<TCommand>
    {
        public IssueCommand(Address address, TCommand command, IVersion expectedVersion)
        {
            Address = address;
            Command = command;
            ExpectedVersion = expectedVersion;
        }
        public Address Address { get; private set; }

        public TCommand Command { get; private set; }

        public IVersion ExpectedVersion { get; private set; }
    }

    internal class Agent<TState, TCommand, TEvent> where TState : Aggregate<TState, TEvent, TCommand>, new()
    {

        public Agent(TState initialState, IEnumerable<TEvent> history, Subject<IssueCommand<TCommand>> subject, SaveEvents<TEvent> saveEvents, GetEventsSince<TEvent> getEventsSince, Conflicts<TCommand, TEvent> isConflicting, IScheduler scheduler)
        {
            var state1 = history.Aggregate(initialState, (state, @event) => state.Apply(@event));

            subject
                .ObserveOn(scheduler)
                .Subscribe(async issueCommand => 
                {                   
                    var eventsSinceExpected = await getEventsSince(issueCommand.Address, issueCommand.ExpectedVersion);
                    var conflicts = eventsSinceExpected.Select(
                        eventDescriptor => isConflicting(issueCommand.Command, eventDescriptor.Event));

                    var transientEvents = state1.Decide(issueCommand.Command);
                    // we depend on the event store in use to determine if there as an optimistic concurrency issue
                    var saveResult = await saveEvents(issueCommand.Address, issueCommand.ExpectedVersion, transientEvents);

                    switch (saveResult)
                    {
                        case Ok<Unit, SaveEventsError> _:
                            // the event stream has been save successfully. Update the state
                            state1 = transientEvents.Aggregate(state1, (state, @event) => state.Apply(@event));
                            break;
                        case Error<Unit, SaveEventsError> (var error):
                            switch(error)
                            {
                                case OptimisticConcurrencyError optimisticConcurrencyError:
                                    // do conflict resolution
                                    // List<Event> eventsSinceExpected = eventstore.GetEventsSince(issueCommand.ExpectedVersion)
                                    // foreach(var @event in eventsSinceExpected)
                                    // {
                                    // if the command and event really conflict according to business logic => log a real OptimisticConcurrencyError
                                    //        if!(Conflict(@event, issueCommand.Command))
                                    break;
                                default: throw new NotSupportedException();                                
                            }
                            break;
                        default: throw new NotSupportedException();
                    }
                });
        }
    }

    /// <summary>
    /// The error to return when saving events causes an optimistic concurrency error
    /// </summary>
    internal class OptimisticConcurrencyError : SaveEventsError
    {
    }

    /// <summary>
    /// The bounded context is responsible for managing the runtime. 
    /// - Once an aggregate is created, it is never destroyed
    /// - An aggreagte can only acquire an address of an other aggregate when it is explicitly send to it
    /// - The runtime is responsible to restoring the state of the aggregate when it is not alife within
    ///   the context or any other remote instance of the context within a cluster
    /// - Only one instance of an aggragate will be alive 
    /// - One can only send commands that are scoped to the bounded context.
    /// - All commands that are scoped to an aggregate MUST be subtypes of the command type scoped at the bounded context level
    /// - There is only one command type scoped and the level of the bounded context.
    ///   All commands scoped to an aggragate MUST be subtypes of that command type
    /// 
    /// </summary>
    /// <typeparam name="TCommand"></typeparam>
    /// <typeparam name="TEvent"></typeparam>
    public class BoundedContext<TCommand, TEvent>
    {
        private readonly SaveEvents<TEvent> _saveEvents;
        private readonly GetEventsSince<TEvent> _getEventsSince;
        private readonly ResolveRemoteAddress _resolveRemoteAddress;
        private readonly Forward<TCommand> _forward;
        private readonly Conflicts<TCommand, TEvent> _conflicts;
        private readonly Dictionary<Address, Subject<IssueCommand<TCommand>>> _registry = new Dictionary<Address, Subject<IssueCommand<TCommand>>>();

        public BoundedContext(SaveEvents<TEvent> saveEvents, GetEventsSince<TEvent> getEventsSince, ResolveRemoteAddress resolveRemoteAddress, Forward<TCommand> forward, Conflicts<TCommand, TEvent> conflicts)
        {
            _saveEvents = saveEvents;
            _getEventsSince = getEventsSince;
            _resolveRemoteAddress = resolveRemoteAddress;
            _forward = forward;
            _conflicts = conflicts;
        }

        
        internal Address CreateAggregate<TState>(IScheduler scheduler) where TState : Aggregate<TState, TEvent, TCommand>, new()
        {
            var history = new List<TEvent>();
            var address = new Address(Guid.NewGuid());
            Subject<IssueCommand<TCommand>> subject = new Subject<IssueCommand<TCommand>>();
            var agent = new Agent<TState, TCommand, TEvent>(new TState(), history, subject, _saveEvents, _getEventsSince, _conflicts, scheduler);
            _registry.Add(address, subject);
            return address;
        }

        internal void Send(TCommand command, IVersion expectedVersion, Address address)
        {
            
            var subject = _registry[address];
            subject.OnNext(new IssueCommand<TCommand>(address, command, expectedVersion));            
        }

        /// <summary>
        /// Creates a new aggregate that schedules work using the threadpool
        /// </summary>
        /// <typeparam name="TState"></typeparam>
        /// <returns></returns>
        internal Address CreateAggregate<TState>() where TState : Aggregate<TState, TEvent, TCommand>, new()
        {
            return CreateAggregate<TState>(ThreadPoolScheduler.Instance);
        }
    }

    /// <summary>
    /// An error while save to the event store
    /// </summary>
    public interface SaveEventsError {}

    public interface ResolveRemoteAddressError {}

    public interface ForwardError{}



    /// <summary>
    /// 
    /// </summary>
    /// <param name="address">The address of the aggregate</param>
    /// <param name="expectedVersion">The version the event stream is expected to be at when adding the new events.true For the purpose of optimistic concurrency</param>
    /// <param name="events"></param>
    /// <typeparam name="TEvent">The type of events</typeparam>
    /// <returns>Either a confirmation the action succeeded (in the for of an instance of Unit) or an error. The SaveEvents error is on if the following:
    /// - OptimisticConcurrencyError
    /// </returns>
    public delegate Task<Result<Unit, SaveEventsError>> SaveEvents<in TEvent>(Address address, IVersion expectedVersion, IEnumerable<TEvent> events);

    /// <summary>
    /// Get all event descriptors for an aggregate since (excluding) the supplied version
    /// </summary>
    /// <param name="address">Address of the aggregate</param>
    /// <param name="version"></param>
    /// <typeparam name="TEvent"></typeparam>
    /// <returns></returns>
    public delegate Task<List<EventDescriptor<TEvent>>> GetEventsSince<TEvent>(Address address, IVersion version);

    /// <summary>
    /// Combining the metadata of an event with the even itself
    /// </summary>
    public struct EventDescriptor<TEvent>
    {
        private readonly TEvent _event;
        private readonly Version _version;

        public EventDescriptor(TEvent @event, Version version)
        {
            _event = @event;
            _version = version;
        }

        public TEvent Event => _event;

        public Version Version => _version;
    }

    /// <summary>
    /// Returns the uri of the resource hosting the aggregate with the indicated address
    /// todo : will shift to the multiaddress spec https://github.com/multiformats/multiaddr for addressing
    /// </summary>
    /// <param name="address"></param>
    /// <returns>Either the Uri or an error</returns>
    public delegate Task<Result<Uri, ResolveRemoteAddressError>> ResolveRemoteAddress(Address address);

    /// <summary>
    /// Forwards a command to an other context
    /// </summary>
    /// <param name="command"></param>
    /// <param name="destination"></param>
    /// <param name="address"></param>
    /// <typeparam name="TCommand"></typeparam>
    /// <returns></returns>
    public delegate Task<Result<Unit, ForwardError>> Forward<TCommand>(TCommand command, Uri destination, Address address);

    /// <summary>
    /// Returns the first conflict between the comment and an event, if any
    /// </summary>
    /// <param name="command"></param>
    /// <param name="event"></param>
    /// <typeparam name="TCommand"></typeparam>
    /// <typeparam name="TEvent"></typeparam>
    /// <returns></returns>
    public delegate Maybe<Conflict<TCommand, TEvent>> Conflicts<TCommand, TEvent>(TCommand command, TEvent @event);

    public struct Conflict<TCommand, TEvent>
    {
        public Conflict(TCommand command, TEvent @event, string reason)
        {
            Command = command;
            Event = @event;
            Reason = reason;
        }

        public TCommand Command { get; }
        public TEvent Event { get; }
        public string Reason { get; }
    }

    public class AggregateProperties
    {
        [Property(DisplayName = "Given an inventory item was created previously and we are disregarding concurrency conflicts, and items are checked into the inventory, the expected event should be added to the stream")]
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
            GetEventsSince<InventoryItemEvent> getEventsSince = (_,__) => Task.FromResult(new List<EventDescriptor<InventoryItemEvent>>());
            Conflicts<InventoryItemCommand, InventoryItemEvent> conflicts = (_, __) => Maybe.None<Conflict<InventoryItemCommand, InventoryItemEvent>>.Default;

            var context = new BoundedContext<InventoryItemCommand, InventoryItemEvent>(saveEvents, getEventsSince, resolveRemoteAddress, forward);
            // for testing purposes make the aggregate block the current thread while processing
            var inventoryItem = context.CreateAggregate<InventoryItem>(Scheduler.CurrentThread);

            context.Send(new CreateInventoryItem("Product 1"), new AnyVersion(), inventoryItem);
            context.Send(new CheckInItemsToInventory(amount.Get), new AnyVersion(), inventoryItem);

            eventStream.Should().Equal(new List<InventoryItemEvent>{ new InventoryItemCreated("Product 1"), new ItemsCheckedInToInventory(amount.Get)});         
            
        }

        [Property(
            DisplayName = "Given there is a concurrency conflict and conflict resolution determines that there truly is a conflict, the last command should be disregarded")]
        public void Property2(PositiveInt amount)
        {
            var eventStream = new List<InventoryItemEvent>();
            SaveEvents<InventoryItemEvent> saveEvents = (_, __, events) => 
            {
                eventStream.AddRange(events);
                return Task.FromResult(Ok<Unit, SaveEventsError>(Unit.Instance));
            };

            ResolveRemoteAddress resolveRemoteAddress = address => Task.FromResult(Ok<Uri, ResolveRemoteAddressError>(new Uri("")));
            Forward<InventoryItemCommand> forward = (_, __, ___) => Task.FromResult(Ok<Unit, ForwardError>(Unit.Instance));
            GetEventsSince<InventoryItemEvent> getEventsSince = (_,__) => Task.FromResult(new List<EventDescriptor<InventoryItemEvent>>
            {
                new EventDescriptor<InventoryItemEvent>(new InventoryItemCreated("Product 1"), 1L),
                new EventDescriptor<InventoryItemEvent>(new ItemsCheckedInToInventory(amount.Get), 2L),
                new EventDescriptor<InventoryItemEvent>(new InventoryItemRenamed("Product 2"), 3L)
            });

            var context = new BoundedContext<InventoryItemCommand, InventoryItemEvent>(saveEvents, getEventsSince, resolveRemoteAddress, forward);
            // for testing purposes make the aggregate block the current thread while processing
            var inventoryItem = context.CreateAggregate<InventoryItem>(Scheduler.CurrentThread);

            Version expectedVersion = 2L;    
            context.Send(new CheckInItemsToInventory(amount.Get), expectedVersion, inventoryItem);

            eventStream.Should().Equal(new List<InventoryItemEvent>{ }, "no event should be saved to the event store given the command was discarded");         
        }

              [Property(DisplayName = "Given there is a concurrency conflict and conflict resolution waves the conflict, the expected event should be added to the stream")]
        public void Property3(PositiveInt amount)
        {
            var eventStream = new List<InventoryItemEvent>();
            SaveEvents<InventoryItemEvent> saveEvents = (_, __, events) => 
            {
                eventStream.AddRange(events);
                return Task.FromResult(Ok<Unit, SaveEventsError>(Unit.Instance));
            };
            GetEventsSince<InventoryItemEvent> getEventsSince = (_,__) => Task.FromResult(new List<EventDescriptor<InventoryItemEvent>>());
            ResolveRemoteAddress resolveRemoteAddress = address => Task.FromResult(Ok<Uri, ResolveRemoteAddressError>(new Uri("")));
            Forward<InventoryItemCommand> forward = (_, __, ___) => Task.FromResult(Ok<Unit, ForwardError>(Unit.Instance));

            var context = new BoundedContext<InventoryItemCommand, InventoryItemEvent>(saveEvents, getEventsSince, resolveRemoteAddress, forward);
            // for testing purposes make the aggregate block the current thread while processing
            var inventoryItem = context.CreateAggregate<InventoryItem>(Scheduler.CurrentThread);

            Version version1 = 1L;

            context.Send(new CreateInventoryItem("Product 1"), version1, inventoryItem);
            context.Send(new CheckInItemsToInventory(amount.Get), version1, inventoryItem);

            eventStream.Should().Equal(new List<InventoryItemEvent>{ new InventoryItemCreated("Product 1"), new ItemsCheckedInToInventory(amount.Get)});         
            
        }

        [Property(DisplayName = "Given there is no concurrency conflict, the expected event should be added to the stream")]
        public void Property4(PositiveInt amount)
        {
            var eventStream = new List<InventoryItemEvent>();
            SaveEvents<InventoryItemEvent> saveEvents = (_, __, events) => 
            {
                eventStream.AddRange(events);
                return Task.FromResult(Ok<Unit, SaveEventsError>(Unit.Instance));
            };
            GetEventsSince<InventoryItemEvent> getEventsSince = (_,__) => Task.FromResult(new List<EventDescriptor<InventoryItemEvent>>());
            ResolveRemoteAddress resolveRemoteAddress = address => Task.FromResult(Ok<Uri, ResolveRemoteAddressError>(new Uri("")));
            Forward<InventoryItemCommand> forward = (_, __, ___) => Task.FromResult(Ok<Unit, ForwardError>(Unit.Instance));

            var context = new BoundedContext<InventoryItemCommand, InventoryItemEvent>(saveEvents, getEventsSince, resolveRemoteAddress, forward);
            // for testing purposes make the aggregate block the current thread while processing
            var inventoryItem = context.CreateAggregate<InventoryItem>(Scheduler.CurrentThread);

            Version version1 = 1L;
            Version version2 = 2L;

            context.Send(new CreateInventoryItem("Product 1"), version1, inventoryItem);
            context.Send(new CheckInItemsToInventory(amount.Get), version2, inventoryItem);

            eventStream.Should().Equal(new List<InventoryItemEvent>{ new InventoryItemCreated("Product 1"), new ItemsCheckedInToInventory(amount.Get)});         
            
        }
    }
}