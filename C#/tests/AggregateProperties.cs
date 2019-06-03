using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static Radix.Tests.Result.Extensions;
using Radix.Tests.Result;
using FsCheck.Xunit;
using FsCheck;
using System.Linq;
using System.Threading.Tasks.Dataflow;
using FluentAssertions;

namespace Radix.Tests
{

    public interface IVersion
    {

    }

    public class Version : IVersion, IComparable<Version>
    {
        private long Value { get; }

        private Version(long value)
        {
            Value = value;
            ;
        }

        public static implicit operator Version(long value)
        {
            return new Version(value);
        }

        public static implicit operator long(Version version)
        {
            return version.Value;
        }

        public int CompareTo(Version other) => Value.CompareTo(other);
    }

    public class AnyVersion : IVersion
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
        /// <summary>
        /// This is the place to validate a command and decide of any events will be generated as a
        /// consequence of this command. You should not and are not allowed to change the state here
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        List<TEvent> Decide(TCommand command);

        /// <summary>
        /// Here the effect of the event on the state of the aggregate is determined.
        /// The new state will be returned as an effect
        /// </summary>
        TState Apply(TEvent @event);
    }

    public class InventoryItem : Aggregate<InventoryItem, InventoryItemEvent, InventoryItemCommand>
    {
        private string Name { get; }
        private bool Activated { get; }
        private int Count { get; }

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

    public class InventoryItemDeactivated : InventoryItemEvent { }

    public class InventoryItemCreated : InventoryItemEvent
    {
        protected bool Equals(InventoryItemCreated other)
        {
            return string.Equals(Name, other.Name);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((InventoryItemCreated) obj);
        }

        public override int GetHashCode()
        {
            return (Name != null ? Name.GetHashCode() : 0);
        }

        public string Name { get; }

        public InventoryItemCreated(string name)
        {
            Name = name;

        }
    }

    public class InventoryItemRenamed : InventoryItemEvent
    {
        public string Name { get; }

        public InventoryItemRenamed(string name)
        {
            Name = name;

        }
    }

    public class ItemsCheckedInToInventory : InventoryItemEvent
    {
        protected bool Equals(ItemsCheckedInToInventory other)
        {
            return Amount == other.Amount;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((ItemsCheckedInToInventory) obj);
        }

        public override int GetHashCode()
        {
            return Amount;
        }

        public int Amount { get; }

        public ItemsCheckedInToInventory(int amount)
        {
            Amount = amount;

        }
    }

    public class ItemsRemovedFromInventory : InventoryItemEvent
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


    public class CreateInventoryItem : InventoryItemCommand
    {
        public string Name { get; }

        public CreateInventoryItem(string name)
        {
            Name = name;
        }
    }

    public class RenameInventoryItem : InventoryItemCommand
    {
        public string Name { get; }

        public RenameInventoryItem(string name)
        {
            Name = name;
        }
    }

    public class CheckInItemsToInventory : InventoryItemCommand
    {
        public int Amount { get; }

        public CheckInItemsToInventory(int amount)
        {
            Amount = amount;
        }
    }

    public class RemoveItemsFromInventory : InventoryItemCommand
    {
        public int Amount { get; }

        public RemoveItemsFromInventory(int amount)
        {
            Amount = amount;
        }
    }

    public class CommandDescriptor<TCommand>
    {
        public CommandDescriptor(Address address, TCommand command, IVersion expectedVersion)
        {
            Address = address;
            Command = command;
            ExpectedVersion = expectedVersion;
        }
        public Address Address { get; }

        public TCommand Command { get; }

        public IVersion ExpectedVersion { get; }
    }



    /// <summary>
    /// The error to return when saving events causes an optimistic concurrency error
    /// </summary>
    internal class OptimisticConcurrencyError : SaveEventsError
    {
    }

    public class BoundedContextSettings<TCommand, TEvent>
    {
        public BoundedContextSettings(SaveEvents<TEvent> saveEvents, GetEventsSince<TEvent> getEventsSince, ResolveRemoteAddress resolveRemoteAddress, Forward<TCommand> forward, FindConflicts<TCommand, TEvent> findConflicts)
        {
            SaveEvents = saveEvents;
            GetEventsSince = getEventsSince;
            ResolveRemoteAddress = resolveRemoteAddress;
            Forward = forward;
            FindConflicts = findConflicts;
        }

        public SaveEvents<TEvent> SaveEvents { get; }
        public GetEventsSince<TEvent> GetEventsSince { get; }
        public ResolveRemoteAddress ResolveRemoteAddress { get; private set; }
        public Forward<TCommand> Forward { get; private set; }
        public FindConflicts<TCommand, TEvent> FindConflicts { get; private set; }
    }

    internal interface Agent<TCommand>
    {
        void Post(CommandDescriptor<TCommand> commandDescriptor);
    }

    internal class StatefulAgent<TState, TCommand, TEvent> : Agent<TCommand> where TState : Aggregate<TState, TEvent, TCommand>, new()
    {
        // ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable => prevent implicit closure
        private readonly BoundedContextSettings<TCommand, TEvent> _boundedContextSettings;
        private readonly ActionBlock<CommandDescriptor<TCommand>> _actionBlock;
        private TState _state = new TState();

        public StatefulAgent(BoundedContextSettings<TCommand, TEvent> boundedContextSettings, TaskScheduler scheduler)
        {
            _boundedContextSettings = boundedContextSettings;

            _actionBlock = new ActionBlock<CommandDescriptor<TCommand>>(
                async commandDescriptor =>
                {
                    var expectedVersion = commandDescriptor.ExpectedVersion;
                    var eventsSinceExpected = await _boundedContextSettings.GetEventsSince(commandDescriptor.Address, expectedVersion);
                    if (eventsSinceExpected.Any())
                    {
                        var conflicts = _boundedContextSettings.FindConflicts(commandDescriptor.Command, eventsSinceExpected);
                        if (conflicts.Any())
                        {
                            // a true concurrent exception according to business logic
                            // todo : notify the issuer of the command
                            return;
                        }
                        // no conflicts, set the expected version
                        expectedVersion = eventsSinceExpected.Select(eventDescriptor => eventDescriptor.Version).Max();
                    }

                    var transientEvents = _state.Decide(commandDescriptor.Command);
                    // try to save the events
                    var saveResult = await _boundedContextSettings.SaveEvents(commandDescriptor.Address, expectedVersion, transientEvents);

                    switch (saveResult)
                    {
                        case Ok<Unit, SaveEventsError> _:
                            // the events have been saved to the stream successfully. Update the state
                            _state = transientEvents.Aggregate(_state, (s, @event) => s.Apply(@event));
                            break;
                        case Error<Unit, SaveEventsError>(var error):
                            switch (error)
                            {
                                case OptimisticConcurrencyError _:
                                    // re issue the command to try again
                                    Post(commandDescriptor);
                                    break;
                                default: throw new NotSupportedException();
                            }
                            break;
                        default: throw new NotSupportedException();
                    }
                }, new ExecutionDataflowBlockOptions
                {
                    TaskScheduler = scheduler
                });
        }
        
        public void Post(CommandDescriptor<TCommand> commandDescriptor)
        {
            _actionBlock.Post(commandDescriptor);
        }
    }

    /// <summary>
    /// The bounded context is responsible for managing the runtime. 
    /// - Once an aggregate is created, it is never destroyed
    /// - An aggregate can only acquire an address of an other aggregate when it is explicitly send to it or it has created it
    /// - The runtime is responsible to restoring the state of the aggregate when it is not alife within
    ///   the context or any other remote instance of the context within a cluster
    /// - Only one instance of an aggregate will be alive 
    /// - One can only send commands that are scoped to the bounded context.
    /// - All commands that are scoped to an aggregate MUST be subtypes of the command type scoped at the bounded context level
    /// - There is only one command type scoped and the level of the bounded context.
    ///   All commands scoped to an aggregate MUST be subtypes of that command type
    /// 
    /// </summary>
    /// <typeparam name="TCommand"></typeparam>
    /// <typeparam name="TEvent"></typeparam>
    public class BoundedContext<TCommand, TEvent>
    {
        private readonly BoundedContextSettings<TCommand, TEvent> _boundedContextSettings;
        private readonly Dictionary<Address, Agent<TCommand>> _registry = new Dictionary<Address, Agent<TCommand>>();

        public BoundedContext(BoundedContextSettings<TCommand, TEvent> boundedContextSettings)
        {
            _boundedContextSettings = boundedContextSettings;
        }

        
        internal Address CreateAggregate<TState>(TaskScheduler scheduler) where TState : Aggregate<TState, TEvent, TCommand>, new()
        {
            var address = new Address(Guid.NewGuid());

            var agent = new StatefulAgent<TState,TCommand,TEvent>(_boundedContextSettings, scheduler );

            _registry.Add(address, agent);
            return address;
        }

        internal void Send(TCommand command, IVersion expectedVersion, Address address)
        {
            
            var agent = _registry[address];
            agent.Post(new CommandDescriptor<TCommand>(address, command, expectedVersion));            
        }

        /// <summary>
        /// Creates a new aggregate that schedules work using the default task scheduler (TaskScheduler.Default)
        /// </summary>
        /// <typeparam name="TState"></typeparam>
        /// <returns></returns>
        internal Address CreateAggregate<TState>() where TState : Aggregate<TState, TEvent, TCommand>, new()
        {
            return CreateAggregate<TState>(TaskScheduler.Default);
        }
    }

    /// <summary>
    /// An error while save to the event store
    /// </summary>
    public interface SaveEventsError {}

    public interface ResolveRemoteAddressError {}

    public interface ForwardError {}



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
        public bool Equals(EventDescriptor<TEvent> other)
        {
            return EqualityComparer<TEvent>.Default.Equals(Event, other.Event) && Version.Equals(other.Version);
        }

        public override bool Equals(object obj)
        {
            return obj is EventDescriptor<TEvent> other && Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (EqualityComparer<TEvent>.Default.GetHashCode(Event) * 397) ^ Version.GetHashCode();
            }
        }

        public EventDescriptor(TEvent @event, Version version)
        {
            Event = @event;
            Version = version;
        }

        public TEvent Event { get; }

        public Version Version { get; }

        public static bool operator ==(EventDescriptor<TEvent> left, EventDescriptor<TEvent> right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(EventDescriptor<TEvent> left, EventDescriptor<TEvent> right)
        {
            return !(left == right);
        }
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
    public delegate IEnumerable<Conflict<TCommand, TEvent>> FindConflicts<TCommand, TEvent>(TCommand command, IEnumerable<EventDescriptor<TEvent>> eventDescriptors);

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
            FindConflicts<InventoryItemCommand, InventoryItemEvent> findConflicts = (_, __) => Enumerable.Empty<Conflict<InventoryItemCommand, InventoryItemEvent>>();

            var context = new BoundedContext<InventoryItemCommand, InventoryItemEvent>(new BoundedContextSettings<InventoryItemCommand,InventoryItemEvent>(saveEvents, getEventsSince, resolveRemoteAddress, forward, findConflicts));
            // for testing purposes make the aggregate block the current thread while processing
            var inventoryItem = context.CreateAggregate<InventoryItem>(new CurrentThreadTaskScheduler());

            context.Send(new CreateInventoryItem("Product 1"), new AnyVersion(), inventoryItem);
            context.Send(new CheckInItemsToInventory(amount.Get), new AnyVersion(), inventoryItem);

            eventStream.Should().Equal(new List<InventoryItemEvent>{ new InventoryItemCreated("Product 1"), new ItemsCheckedInToInventory(amount.Get)});         
            
        }

        [Property(
            DisplayName = "Given there is a concurrency conflict and conflict resolution determines that there truly is a conflict, the last command should be disregarded")]
        public void Property2(PositiveInt amount)
        {
            var AppendedEvents = new List<InventoryItemEvent>();
            SaveEvents<InventoryItemEvent> saveEvents = (_, __, events) => 
            {
                AppendedEvents.AddRange(events);
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
            FindConflicts<InventoryItemCommand, InventoryItemEvent> findConflicts = (command, eventDescriptors) => eventDescriptors.Select(descriptor => new Conflict<InventoryItemCommand, InventoryItemEvent>(command, descriptor.Event,"Just because i can"));

            var context = new BoundedContext<InventoryItemCommand, InventoryItemEvent>(new BoundedContextSettings<InventoryItemCommand,InventoryItemEvent>(saveEvents, getEventsSince, resolveRemoteAddress, forward, findConflicts));
            // for testing purposes make the aggregate block the current thread while processing
            var inventoryItem = context.CreateAggregate<InventoryItem>(new CurrentThreadTaskScheduler());

            Version expectedVersion = 2L;    
            context.Send(new CheckInItemsToInventory(amount.Get), expectedVersion, inventoryItem);

            AppendedEvents.Should().Equal(new List<InventoryItemEvent>{ }, "no event should be saved to the event store given the command was discarded");         
        }

        [Property(
            DisplayName = "Given there is a concurrency conflict and conflict resolution determines that is NO conflict and a technical concurrency exception arises when appending events to the stream, when retrying the expected event should be added to the stream")]
        public void Property3(PositiveInt amount)
        {
            var calledBefore = false;
            var appendedEvents = new List<InventoryItemEvent>();
            SaveEvents<InventoryItemEvent> saveEvents = (_, __, events) => 
            {
                if(calledBefore)
                {
                    appendedEvents.AddRange(events);
                    return Task.FromResult(Ok<Unit, SaveEventsError>(Unit.Instance));
                }
                calledBefore = true;
                return Task.FromResult(Error<Unit, SaveEventsError>(new OptimisticConcurrencyError()));
            };

            ResolveRemoteAddress resolveRemoteAddress = address => Task.FromResult(Ok<Uri, ResolveRemoteAddressError>(new Uri("")));
            Forward<InventoryItemCommand> forward = (_, __, ___) => Task.FromResult(Ok<Unit, ForwardError>(Unit.Instance));
            GetEventsSince<InventoryItemEvent> getEventsSince = (_,__) =>
            {
                return Task.FromResult(new List<EventDescriptor<InventoryItemEvent>>
                {
                    new EventDescriptor<InventoryItemEvent>(new InventoryItemCreated("Product 1"), 1L),
                    new EventDescriptor<InventoryItemEvent>(new ItemsCheckedInToInventory(amount.Get), 2L),
                    new EventDescriptor<InventoryItemEvent>(new InventoryItemRenamed("Product 2"), 3L)
                });
            };
            FindConflicts<InventoryItemCommand, InventoryItemEvent> findConflicts = (command, eventDescriptors) => Enumerable.Empty<Conflict<InventoryItemCommand, InventoryItemEvent>>();

            var context = new BoundedContext<InventoryItemCommand, InventoryItemEvent>(new BoundedContextSettings<InventoryItemCommand,InventoryItemEvent>(saveEvents, getEventsSince, resolveRemoteAddress, forward, findConflicts));
            // for testing purposes make the aggregate block the current thread while processing
            var inventoryItem = context.CreateAggregate<InventoryItem>(new CurrentThreadTaskScheduler());

            Version expectedVersion = 2L;    
            context.Send(new CheckInItemsToInventory(amount.Get), expectedVersion, inventoryItem);

            appendedEvents.Should().Equal(new List<InventoryItemEvent>{ new ItemsCheckedInToInventory(amount.Get)});  
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
            GetEventsSince<InventoryItemEvent> getEventsSince = (_,__) => Task.FromResult(new List<EventDescriptor<InventoryItemEvent>>
            {
                new EventDescriptor<InventoryItemEvent>(new InventoryItemCreated("Product 1"), 1L),
                new EventDescriptor<InventoryItemEvent>(new ItemsCheckedInToInventory(amount.Get), 2L),
                new EventDescriptor<InventoryItemEvent>(new InventoryItemRenamed("Product 2"), 3L)
            });;
            ResolveRemoteAddress resolveRemoteAddress = address => Task.FromResult(Ok<Uri, ResolveRemoteAddressError>(new Uri("")));
            Forward<InventoryItemCommand> forward = (_, __, ___) => Task.FromResult(Ok<Unit, ForwardError>(Unit.Instance));
            FindConflicts<InventoryItemCommand, InventoryItemEvent> findConflicts = (command, eventDescriptors) => Enumerable.Empty<Conflict<InventoryItemCommand, InventoryItemEvent>>();

            var context = new BoundedContext<InventoryItemCommand, InventoryItemEvent>(new BoundedContextSettings<InventoryItemCommand,InventoryItemEvent>(saveEvents, getEventsSince, resolveRemoteAddress, forward, findConflicts));
            // for testing purposes make the aggregate block the current thread while processing
            var inventoryItem = context.CreateAggregate<InventoryItem>(new CurrentThreadTaskScheduler());

            Version version1 = 2L;

            context.Send(new CheckInItemsToInventory(amount.Get), version1, inventoryItem);

            appendedEvents.Should().Equal(new List<InventoryItemEvent>{ new ItemsCheckedInToInventory(amount.Get)});         
            
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
            GetEventsSince<InventoryItemEvent> getEventsSince = (_,__) => Task.FromResult(new List<EventDescriptor<InventoryItemEvent>>
            {
                new EventDescriptor<InventoryItemEvent>(new InventoryItemCreated("Product 1"), 1L),
                new EventDescriptor<InventoryItemEvent>(new ItemsCheckedInToInventory(amount.Get), 2L),
                new EventDescriptor<InventoryItemEvent>(new InventoryItemRenamed("Product 2"), 3L)
            });
            ResolveRemoteAddress resolveRemoteAddress = address => Task.FromResult(Ok<Uri, ResolveRemoteAddressError>(new Uri("")));
            Forward<InventoryItemCommand> forward = (_, __, ___) => Task.FromResult(Ok<Unit, ForwardError>(Unit.Instance));
            FindConflicts<InventoryItemCommand, InventoryItemEvent> findConflicts = (command, eventDescriptors) => Enumerable.Empty<Conflict<InventoryItemCommand, InventoryItemEvent>>();

            var context = new BoundedContext<InventoryItemCommand, InventoryItemEvent>(new BoundedContextSettings<InventoryItemCommand,InventoryItemEvent>(saveEvents, getEventsSince, resolveRemoteAddress, forward, findConflicts));
            // for testing purposes make the aggregate block the current thread while processing
            var inventoryItem = context.CreateAggregate<InventoryItem>(new CurrentThreadTaskScheduler());

            Version version1 = 1L;

            context.Send(new CreateInventoryItem("Product 1"), version1, inventoryItem);

            appendedEvents.Should().Equal(new List<InventoryItemEvent>{ new InventoryItemCreated("Product 1")});         
            
        }
    }
}
