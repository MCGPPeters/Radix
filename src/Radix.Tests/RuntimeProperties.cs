using System;
using System.Diagnostics;
using System.Net.NetworkInformation;
using FluentAssertions;
using NuGet.Frameworks;
using Radix.Control.Task.Result;
using Radix.Data;
using Radix.Domain.Control;
using Radix.Domain.Data;
using Radix.Inventory.Domain;
using Radix.Inventory.Domain.Data;
using Radix.Inventory.Domain.Data.Commands;
using Radix.Inventory.Domain.Data.Events;
using Radix.Math.Applied.Optimization.Control;
using Radix.Math.Applied.Probability;
using Radix.Math.Pure.Logic.Order.Intervals;
using Radix.Tests.Reinforcement_Learning__an_Introduction.Chapter_4;
using Radix.Tests.Reinforcement_Learning__an_Introduction.Chapter_5;
using SqlStreamStore;
using SqlStreamStore.Streams;
using Xunit;
using Id = Radix.Domain.Data.Aggregate.Id;
using Version = Radix.Domain.Data.Version;
using static Radix.Control.Result.Extensions;

namespace Radix.Tests
{
    public interface Aggregate<TState, in TCommand, TEvent>
        where TState : Aggregate<TState, TCommand, TEvent>, new()
    {
        static abstract string Id { get; }

        static abstract TState Apply(TState state, TEvent @event);

        static abstract IOrderedEnumerable<TEvent> Decide(TState state, TCommand command);

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
        static abstract Task<Result<TEvent[], Error[]>> ResolveConflicts(TState state, IOrderedEnumerable<TEvent> ourEvents, IOrderedAsyncEnumerable<Event<TEvent>> theirEvents);

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
        static abstract Task<Result<ExistingVersion, AppendEventsError>> AppendEvents<TEvent>(Stream eventStream, Version expectedVersion,
            params TEvent[] events);

        static abstract IAsyncEnumerable<Event<TEvent>> GetEvents<TEvent>(Stream eventStream, Closed<Version> interval);
    }

    public record Instance<TState, TCommand, TEvent, TEventStore>
        where TEventStore : EventStore<TEventStore>
        where TState : Aggregate<TState, TCommand, TEvent>, new()
    {
        private Instance()
        {
            
        }

        public required Id Id { get; init; }
        public required TState State { get; init; }
        public required Version Version { get; init; }

        public static async Task<Instance<TState, TCommand, TEvent, TEventStore>> Create()
            =>
               await Get<TEventStore, TState, TCommand, TEvent>(new Id(), new MinimumVersion());

        public static async Task<Instance<TState, TCommand, TEvent, TEventStore>> Get<TEventStore, TState, TCommand, TEvent>(Id instanceId, Version version)
            where TState : Aggregate<TState, TCommand, TEvent>, new()
            where TEventStore : EventStore<TEventStore>
        {
            var state = new TState();
            var stream = new Stream { Id = (StreamId)instanceId.Value, Name = (StreamName)TState.Id };
            var events = TEventStore.GetEvents<TEvent>(stream, new Closed<Version>(new MinimumVersion(), version));
            state = await events.AggregateAsync(state, (current, @event) => TState.Apply(current, @event.Value));

            return new Instance<TState, TCommand, TEvent, TEventStore> { Id = instanceId, State = state, Version = version };
        }

        public async Task<Result<Instance<TState, TCommand, TEvent, TEventStore>, Error[]>> Handle(
            Validated<TCommand> command)
        {
            return command.Select(Handle) switch
            {
                Invalid<Task<Result<Instance<TState, TCommand, TEvent, TEventStore>, Error[]>>>(var invalid) => Error<Instance<TState, TCommand, TEvent, TEventStore>, Error[]>(invalid.SelectMany(reason => reason.Descriptions.Select(description => new Error() { Message = description })).ToArray()),
                Valid<Task<Result<Instance<TState, TCommand, TEvent, TEventStore>, Error[]>>>(var valid) => await valid,
                _ => throw new ArgumentOutOfRangeException()
            };

        }

        private async Task<Result<Instance<TState, TCommand, TEvent, TEventStore>, Error[]>> Handle(TCommand command)
        {
            var stream = new Stream { Id = (StreamId)Id.Value, Name = (StreamName)TState.Id };
            var ourEvents = TState.Decide(State, command);
            var theirEvents = TEventStore.GetEvents<TEvent>(stream, new Closed<Version>(Version, new MaximumVersion())).OrderBy(@event => @event.Version);
            var eventsToAppend = await TState.ResolveConflicts(State, ourEvents, theirEvents);
            var actualState = await theirEvents.AggregateAsync(State, (state, @event) => TState.Apply(state, @event.Value));
            var result = eventsToAppend.Select(async events =>
                from version in await TEventStore.AppendEvents(stream, new AnyVersion(), events)
                let newState = events.Aggregate(actualState, TState.Apply)
                select this with { State = newState, Version = version });

            return result switch
            {
                Error<Task<Result<Instance<TState, TCommand, TEvent, TEventStore>, AppendEventsError>>, Error[]>(var error) =>
                    Error<Instance<TState, TCommand, TEvent, TEventStore>, Error[]>(error),
                Ok<Task<Result<Instance<TState, TCommand, TEvent, TEventStore>, AppendEventsError>>, Error[]>(var ok) =>
                    (await ok).MapError(e => new Error[] { e.Message }),
                _ => throw new ArgumentOutOfRangeException(nameof(result))
            };
        }
    }

    public record Event<TEvent>
    {
        public required Version Version { get; init; }
        public required TEvent Value { get; init; }
    }

    public abstract record Command<TCommand>
    {
        public required TCommand Value { get; init; }

        protected Command()
        {
        }

        public abstract Task<Validated<Command<TCommand>>> Create(TCommand command);
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
        public static Item Apply(Item state, ItemEvent @event) => throw new NotImplementedException();

        public static IOrderedEnumerable<ItemEvent> Decide(Item state, ItemCommand command) => throw new NotImplementedException();

        public static Task<Result<ItemEvent[], Error[]>> ResolveConflicts(Item state, IOrderedEnumerable<ItemEvent> ourEvents, IOrderedAsyncEnumerable<Event<ItemEvent>> theirEvents) => throw new NotImplementedException();
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
            var inventoryItem = await Instance<Item, ItemCommand, ItemEvent, InMemoryEventStore>.Create();
            await Task.Delay(TimeSpan.FromSeconds(1));

            Validated<ItemCommand> validatedRemoveItems = RemoveItemsFromInventory.Create(1, 1);
            var result = await inventoryItem.Handle(validatedRemoveItems);

            switch (result)
            {
                case Ok<Instance<Item, ItemCommand, ItemEvent, InMemoryEventStore>, Error[]>(var instance):
                    instance.State.Should().BeEquivalentTo(new[] { new ItemsRemovedFromInventory(1, 1) });
                    instance.Version.Should().Be(new ExistingVersion(1L));
                    break;
                case Error<Instance<Item, ItemCommand, ItemEvent, InMemoryEventStore>, Error[]>(var error):
                    error.Should().BeNull();
                    break;
            }
        }
    }

    public class InMemoryEventStore : EventStore<InMemoryEventStore>
    {
        public static Task<Result<ExistingVersion, AppendEventsError>> AppendEvents<TEvent>(Stream eventStream, Version expectedVersion, params TEvent[] events) => Task.FromResult(Ok<ExistingVersion, AppendEventsError>(new ExistingVersion(1L)));

        public static IAsyncEnumerable<Event<TEvent>> GetEvents<TEvent>(Stream eventStream, Closed<Version> interval) => AsyncEnumerable.Empty<Event<TEvent>>();
    }
}
