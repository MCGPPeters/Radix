using System.Diagnostics.Contracts;
using System.Text.Json;
using FluentAssertions;
using Radix.Data;
using Radix.Domain.Data;
using Radix.Inventory.Domain.Data.Events;
using Radix.Math.Pure.Logic.Order.Intervals;
using Xunit;
using Version = Radix.Domain.Data.Version;
using Radix.Data.String.Validity;
using Radix.Data.Number.Validity;
using static Radix.Control.Task.Extensions;

namespace Radix.Tests
{
    public interface Aggregate<TState, in TCommand, TEvent>
        where TState : Aggregate<TState, TCommand, TEvent>, new()
    {
        static abstract string Id { get; }

        static abstract TState Apply(TState state, TEvent @event);

        static abstract IEnumerable<TEvent> Decide(TState state, TCommand command);

        /// <summary>
        /// Decide which events need to be emitted that will resolve the conflicting situation.
        ///
        /// This could potentially be an entire new sequence of events, a merger of the two sequences, the result of rebasing the new events on the existing events
        /// or even an empty sequence. Think resolving merge conflicts in git.
        /// </summary>
        /// <param name="state"></param>
        /// <param name="ourEvents"></param>
        /// <param name="theirEvents"></param>
        /// <returns></returns>
        static abstract IAsyncEnumerable<TEvent> ResolveConflicts(TState state, IEnumerable<TEvent> ourEvents, IOrderedAsyncEnumerable<Event<TEvent>> theirEvents);

    }

    [Alias<Guid>]
    public partial struct StreamId : Read<StreamId>
    {
        public static Validated<StreamId> Parse(string s) =>
            Guid
                .TryParse(s, out var guid)
                .Map(succeeded => succeeded ? Valid((StreamId)guid) : Invalid<StreamId>($"The string '{s}' is not a valid Guid"))!;

        public static Validated<StreamId> Parse(string s, string validationErrorMessage) =>
            Guid
                .TryParse(s, out var guid)
                .Map(succeeded => succeeded ? Valid((StreamId)guid) : Invalid<StreamId>(validationErrorMessage))!;
    }

    [Alias<string>]
    public partial record StreamName;

    public record Stream : Show<Stream>
    {
        private const char Separator = '#';

        public required StreamId Id { get; init; }

        public required StreamName Name { get; init; }
        public static string Format(Stream t) => $"{t.Name}{Separator}{t.Id}";

        public static string Format(Stream t, string? format, IFormatProvider? provider) => string.Format(provider, Format(t));

        public static Option<string> Format(Stream t, Option<string> format, Option<IFormatProvider> provider) =>
            from f in format
            from p in provider
            select Format(t, f, p);
    }

    public abstract record ItemCommand : InventoryCommand
    {
    }

    public interface InventoryCommand
    {
    }

    public static class Aggregate
    {

    }

    public interface EventStore<TEventStore>
        where TEventStore : EventStore<TEventStore>
    {
        static abstract Task<ExistingVersion> AppendEvents<TEvent>(Stream eventStream, Version expectedVersion,
            params TEvent[] events);

        static abstract IAsyncEnumerable<Event<TEvent>> GetEvents<TEvent>(Stream eventStream, Closed<Version> interval);
    }

    public static class Extensions
    {
        public static Task<Instance<TState, TCommand, TEvent, TEventStore>>
            Handle<TState, TCommand, TEvent, TEventStore>(this Instance<TState, TCommand, TEvent, TEventStore> instance,
                Validated<TCommand> command)
            where TEventStore : EventStore<TEventStore>
            where TState : Aggregate<TState, TCommand, TEvent>, new()
            => Instance<TState, TCommand, TEvent, TEventStore>.Handle(instance, command);

    }

    public record Instance<TState, TCommand, TEvent, TEventStore>
        where TEventStore : EventStore<TEventStore>
        where TState : Aggregate<TState, TCommand, TEvent>, new()
    {
        private Instance()
        {
            
        }

        public required Domain.Data.Aggregate.Id Id { get; init; }
        public required TState State { get; init; }
        public required Version Version { get; init; }

        public required IEnumerable<TEvent> History { get; init; }

        public static async Task<Instance<TState, TCommand, TEvent, TEventStore>> Create()
            =>
               await Get(new Domain.Data.Aggregate.Id(), new MinimumVersion());

        [Pure]
        public static async Task<Instance<TState, TCommand, TEvent, TEventStore>> Get( Domain.Data.Aggregate.Id instanceId, Version version)
        {
            var state = new TState();
            var stream = new Stream { Id = (StreamId)instanceId.Value, Name = (StreamName)TState.Id };
            var events = TEventStore.GetEvents<TEvent>(stream, new Closed<Version>(new MinimumVersion(), version));
            List<TEvent> history = new();
            state = await events.AggregateAsync(state, (current, @event) =>
            {
                history.Add(@event.Value);
                return TState.Apply(current, @event.Value);
            });

            return new Instance<TState, TCommand, TEvent, TEventStore> { Id = instanceId, State = state, Version = version, History = history};
        }

        [Pure]
        public static Task<Instance<TState, TCommand, TEvent, TEventStore>> Handle(Instance<TState, TCommand, TEvent, TEventStore> instance, Validated<TCommand> command)
        {
            return command.Select(async cmd =>
                {
                    var stream = new Stream { Id = (StreamId)instance.Id.Value, Name = (StreamName)TState.Id };
                    var ourEvents = TState.Decide(instance.State, cmd);
                    var theirEvents = TEventStore.GetEvents<TEvent>(stream, new Closed<Version>(instance.Version, new MaximumVersion())).OrderBy(@event => @event.Version);
                    var eventsToAppend = await TState.ResolveConflicts(instance.State, ourEvents, theirEvents).ToArrayAsync();
                    var actualState = await theirEvents.AggregateAsync(instance.State, (state, @event) => TState.Apply(state, @event.Value));
                    var version = await TEventStore.AppendEvents(stream, new AnyVersion(), eventsToAppend);
                    var newState = eventsToAppend.Aggregate(actualState, TState.Apply);
                    return instance with { State = newState, Version = version, History = instance.History.Concat(eventsToAppend) };
                }) switch
                {
                    Invalid<Task<Instance<TState, TCommand, TEvent, TEventStore>>> (var reasons) => Task.FromException<Instance<TState, TCommand, TEvent, TEventStore>>(new ValidationErrorException(reasons)),
                    Valid<Task<Instance<TState, TCommand, TEvent, TEventStore>>>(var valid) => valid,
                    _ => throw new ArgumentOutOfRangeException()
                } ;
        }
    }

    public class ValidationErrorException : Exception
    {
        public Reason[] Reasons { get; }

        public ValidationErrorException(params Reason[] reasons)
        {
            Reasons = reasons;
        }
    }

    public record Event<TEvent>
    {
        public required Version Version { get; init; }
        public required string EventType { get; init; }
        public required TEvent Value { get; init; }
    }

    public record Command<TCommand>
    {
        public required TCommand Value { get; init; }

        private Command()
        {
        }

        public static async Task<Validated<Command<TCommand>>> Create(TCommand command, Func<TCommand, Task<Validated<TCommand>>> f)
        {
            var validatedCommand = await f(command);
            return validatedCommand.Select(cmd => new Command<TCommand>{Value = cmd});
        }
    }

    public record DeactivateItem(string Reason) : ItemCommand
    {

        private static Func<string, ItemCommand> New => (reason) =>
            new DeactivateItem(reason);

        public static Validated<ItemCommand> Create(string? reason) => Valid(New)
            .Apply(!string
                .IsNullOrEmpty(reason)
                ? Valid(reason)
                : Invalid<string>("Reason", "A reason for deactivation must be provided"));
    }

    [Validated<long, IsGreaterThanZero<long>>]
    public partial record Id
    {

    }

    public record CreateItem : ItemCommand
    {
        private CreateItem(Id id, string name, bool activated, int count)
        {
            this.Id = id;
            this.Name = name;
            this.Activated = activated;
            this.Count = count;
        }

        private static Func<Id, string, bool, int, ItemCommand> New => (id, name, activated, count) =>
            new CreateItem(id, name, activated, count);

        public Id Id { get; }
        public string Name { get; }
        public bool Activated { get; }
        public int Count { get; }

        public static Validated<ItemCommand> Create(long id, string name, bool activated, int count) => Valid(New)
            .Apply(Id.Create(id))
            .Apply(IsNotNullOrEmpty.Validate("Name")(name))
            .Apply(Valid(activated))
            .Apply(
                count > 0
                    ? Valid(count)
                    : Invalid<int>("Count", "A new inventory item should have at least 1 instance"));
    }

    public record RenameItem(long Id, string Name) : ItemCommand;

    public record CheckInItemsToInventory(long Id, int Amount) : ItemCommand
    {
        private static Func<long, int, ItemCommand> New => (id, amount) =>
            new CheckInItemsToInventory(id, amount);

        public static Validated<ItemCommand> Create(long id, int amount) =>
            Valid(New)
                .Apply(
                    id > 0
                        ? Valid(id)
                        : Invalid<long>("Inventory item id", $"The id of the inventory item must be greater than 0 but is '{id}'"))
                .Apply(
                    amount > 0
                        ? Valid(amount)
                        : Invalid<int>("Amount", $"The amount of the inventory item must be greater than 0 but is '{amount}'"));
    }

    public record Item : Aggregate<Item, ItemCommand, ItemEvent>
    {
        public Item()
        {
            Name = "";
            Activated = true;
            Count = 0;
            ReasonForDeactivation = "";
        }

        public Item(string name, bool activated, int count)
        {
            Name = name;
            Activated = activated;
            Count = count;
            ReasonForDeactivation = "";
        }

        public string? ReasonForDeactivation { get; init; }
        public string? Name { get; init; }
        public bool Activated { get; init; }
        public int Count { get; init; }
        public static string Id { get; }
        public static Item Apply(Item state, ItemEvent @event) =>
            @event switch
        {
            ItemCreated inventoryItemCreated => state with { Name = inventoryItemCreated.Name, Count = inventoryItemCreated.Count},
            ItemDeactivated inventoryItemDeactivated => state with { Activated = false, ReasonForDeactivation = inventoryItemDeactivated.Reason },
            ItemsCheckedInToInventory itemsCheckedInToInventory => state with { Count = state.Count + itemsCheckedInToInventory.Amount },
            ItemsRemovedFromInventory itemsRemovedFromInventory => state with { Count = state.Count - itemsRemovedFromInventory.Amount },
            ItemRenamed inventoryItemRenamed => state with { Name = inventoryItemRenamed.Name },
            _ => throw new NotSupportedException("Unknown event")
        };

        public static IEnumerable<ItemEvent> Decide(Item state, ItemCommand command) =>
            command switch
        {
            DeactivateItem deactivateInventoryItem => new ItemEvent[] { new ItemDeactivated(deactivateInventoryItem.Reason) },
            CreateItem createInventoryItem =>
                new ItemEvent[] {new ItemCreated(createInventoryItem.Id, createInventoryItem.Name, createInventoryItem.Activated, createInventoryItem.Count)},
            RenameItem renameInventoryItem =>
                    new ItemEvent[] { new ItemRenamed { Id = renameInventoryItem.Id, Name = renameInventoryItem.Name } },
            CheckInItemsToInventory checkInItemsToInventory => 
                    new ItemEvent[] { new ItemsCheckedInToInventory { Amount = checkInItemsToInventory.Amount, Id = checkInItemsToInventory.Id }} ,
            RemoveItemsFromInventory removeItemsFromInventory => 
                    new ItemEvent[] { new ItemsRemovedFromInventory(removeItemsFromInventory.Amount, removeItemsFromInventory.Id) },
            _ => throw new NotSupportedException("Unknown transientCommand")
        };

        public static IAsyncEnumerable<ItemEvent> ResolveConflicts(Item state, IEnumerable<ItemEvent> ourEvents, IOrderedAsyncEnumerable<Event<ItemEvent>> theirEvents) => ourEvents.ToAsyncEnumerable();
    }

    public record RemoveItemsFromInventory : ItemCommand
    {
        public long Id { get; }
        public int Amount { get; }

        private RemoveItemsFromInventory(long id, int amount)

        {
            Id = id;
            Amount = amount;
        }

        private static Func<long, int, ItemCommand> New => (id, amount) =>
            new RemoveItemsFromInventory(id, amount);


        public static Validated<ItemCommand> Create(long id, int amount) => Valid(New)
            .Apply(
                id > 0
                    ? Valid(id)
                    : Invalid<long>("Id", ""))
            .Apply(
                amount > 0
                    ? Valid(amount)
                    : Invalid<int>("Amount", ""));
    }

    public class RuntimeProperties
    {

        [Fact(
            DisplayName =
                "Given an instance of an aggregate is not active, but it does exist, when sending a command it should be restored to the correct state and process the command")]
        public async Task Test1()
        {

            // for testing purposes make the aggregate block the current thread while processing
            var inventoryItem = await Instance<Item, ItemCommand, ItemEvent, EventStoreStub>.Create();
            await Task.Delay(TimeSpan.FromSeconds(1));

            var validatedCreateItem = CreateItem.Create(1, "Product 1", true, 5);
            var validatedCheckinItems = CheckInItemsToInventory.Create(1, 19);
            var validatedRenameItem = Valid((ItemCommand) new RenameItem(1, "Product 2"));
            var validatedRemoveItems = RemoveItemsFromInventory.Create(1, 1);



            var handle = await
                from i1 in inventoryItem.Handle(validatedCreateItem)
                from i2 in i1.Handle(validatedCheckinItems)
                from i3 in i2.Handle(validatedRenameItem)
                select i3.Handle(validatedRemoveItems);

            var instance = await handle;
            instance.History.Should().BeEquivalentTo(new List<ItemEvent>
            {
                new ItemCreated(1, "Product 1", true, 5),
                new ItemsCheckedInToInventory {Amount = 19, Id = 1},
                new ItemRenamed {Name = "Product 2", Id = 1},
                new ItemsRemovedFromInventory(1, 1)
            }, options => options.RespectingRuntimeTypes());
            instance.Version.Should().Be(new ExistingVersion(4L));
            instance.State.Should().BeEquivalentTo(new Item { Activated = true, Count = 23, Name = "Product 2" });

        }
    }

    public class EventStoreStub : EventStore<EventStoreStub>
    {
        private static readonly List<string> serializedEvents = new List<string> { };
        public static long currentVersion = 0;

        public static JsonSerializerOptions options = new() { Converters = { new PolymorphicWriteOnlyJsonConverter<InventoryEvent>() } };

        public static Task<ExistingVersion> AppendEvents<TEvent>(Stream eventStream,
            Version expectedVersion, params TEvent[] events)
        {
            foreach (TEvent @event in events)
            {
                serializedEvents.Add(JsonSerializer.Serialize(@event, options));
                currentVersion++;
            }
            
            return Task.FromResult(new ExistingVersion(currentVersion));
        }

        public static TEvent Deserialize<TEvent>(string json)=> JsonSerializer.Deserialize<TEvent>(json, options);
        public static async IAsyncEnumerable<Event<TEvent>> GetEvents<TEvent>(Stream eventStream, Closed<Version> interval)

        {
            long version = 0;
            foreach (string serializedEvent in serializedEvents)
            {
                version++;
                yield return new Event<TEvent>
                {
                    Value = Deserialize<TEvent>(serializedEvent),
                    EventType = typeof(TEvent).FullName, 
                    Version = new ExistingVersion(version)
                };
            }
        }
    }
}
