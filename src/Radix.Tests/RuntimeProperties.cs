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
        where TCommand : Command<TCommand>
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
             

        static virtual TState Handle(TState state, TCommand command)
            => TState
                .Decide(state, command)
                .Aggregate(state, TState.Apply);
    }

    public static class Instance
    {
        public static async Task<Instance<TState>> Create<TEventStore, TState, TCommand, TEvent>()
            where TCommand : Command<TCommand>
            where TState : Aggregate<TState, TCommand, TEvent>, new()
            where TEventStore : EventStore<TEventStore> =>
                await Get<TEventStore, TState, TCommand, TEvent>(new Id(), new MinimumVersion());

        public static async Task<Instance<TState>> Get<TEventStore, TState, TCommand, TEvent>(Id instanceId, Version version)
            where TCommand : Command<TCommand>
            where TState : Aggregate<TState, TCommand, TEvent>, new()
            where TEventStore : EventStore<TEventStore>
        {
            var state = new TState();
            var stream = new Stream { Id = (StreamId) instanceId.Value, Name = (StreamName) TState.Id };
            var events = TEventStore.GetEvents<TEvent>(stream, new Closed<Version>(new MinimumVersion(), version));
            state = await events.AggregateAsync(state, (current, @event) => TState.Apply(current, @event.Value));

            return new Instance<TState>() {Id = instanceId, State = state, Version = version};
        }

        public static async Task<Result<Instance<TState>, Error[]>> Handle<TEventStore, TState, TCommand, TEvent>(this Instance<TState> instance, TCommand command)
            where TCommand : Command<TCommand>
            where TState : Aggregate<TState, TCommand, TEvent>, new()
            where TEventStore : EventStore<TEventStore>
        {
            var stream = new Stream { Id = (StreamId)instance.Id.Value, Name = (StreamName)TState.Id };
            var ourEvents = TState.Decide(instance.State, command);
            var theirEvents = TEventStore.GetEvents<TEvent>(stream, new Closed<Version>(instance.Version, new MaximumVersion())).OrderBy(@event => @event.Version);
            var eventsToAppend = await TState.ResolveConflicts(instance.State, ourEvents, theirEvents);
            var actualState = await theirEvents.AggregateAsync(instance.State, (state, @event) => TState.Apply(state, @event.Value));
            var result = eventsToAppend.Select(async events =>
                from version in await TEventStore.AppendEvents(stream, new AnyVersion(), events)
                let newState = events.Aggregate(actualState, TState.Apply)
                select instance with { State = newState, Version = version });

            return result switch
            {
                Error<Task<Result<Instance<TState>, AppendEventsError>>, Error[]> (var error) =>
                    Error<Instance<TState>, Error[]>(error),
                Ok<Task<Result<Instance<TState>, AppendEventsError>>, Error[]> (var ok) =>
                    (await ok).MapError(e => new Error[]{ e.Message}),
                _ => throw new ArgumentOutOfRangeException(nameof(result))
            };
        }
    }



    [Alias<DeterministicGuid>]
    public partial struct StreamId : Read<StreamId>
    {
        public static Validated<StreamId> Parse(string s) =>
            DeterministicGuid
                .Parse(s)
                .Map(streamId => (StreamId)streamId);

        public static Validated<StreamId> Parse(string s, string validationErrorMessage) =>
            DeterministicGuid
                .Parse(s, validationErrorMessage)
                .Map(streamId => (StreamId)streamId);
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

    public abstract record ItemCommand : Command<ItemCommand>, InventoryCommand
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

    public record Instance<TState>
    {
        public required Id Id { get; init; }
        public required TState State { get; init; }
        public required Version Version { get; init; }
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

    public record Item : Aggregate<Item>
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
    }

    public class RuntimeProperties
    {

        [Fact(
            DisplayName =
                "Given an instance of an aggregate is not active, but it does exist, when sending a command it should be restored to the correct state and process the command")]
        public async Task Test1()
        {

            Context<InventoryCommand, InventoryEvent, Json> context = new TestInventoryBoundedContext();
            // for testing purposes make the aggregate block the current thread while processing
            var inventoryItem = context.Create<Item, ItemCommandHandler>();
            await Task.Delay(TimeSpan.FromSeconds(1));

            Validated<InventoryCommand> validatedRemoveItems = RemoveItemsFromInventory.Create(1, 1);
            var result = validatedRemoveItems.Select(async removeItems =>
            {
                 var x = await inventoryItem(removeItems);
                 switch (x)
                 {
                     case Ok<CommandResult<ItemEvent>, Error>(var commandResult):
                         commandResult.Events.Should().BeEquivalentTo(new[] { new ItemsRemovedFromInventory(1, 1) });
                         commandResult.ExpectedVersion.Should().Be(new ExistingVersion(4L));
                         break;
                     case Error<CommandResult<ItemEvent>, Error>(var error):
                         error.Should().BeNull();
                         break;
                 }
            });
        }

        [Fact(DisplayName = "Given an instance of an aggregate is not active, but it does exist, when sending a command it should be restored to the correct state and process the command")]
        public async Task Test2()
        {
            var create = Aggregate.Create<InMemoryEventStore, Item, ItemCommand, ItemEvent>()(getEventsSince)();
        }
    }

    public class InMemoryEventStore : EventStore<InMemoryEventStore>
    {
        public Task<Result<ExistingVersion, AppendEventsError>> AppendEvents<TFormat>(Stream eventStream, Version expectedVersion,
            params TransientEventDescriptor<TFormat>[] transientEventDescriptors) =>
            throw new NotImplementedException();

        public IAsyncEnumerable<Stream> GetEventsSince(Stream eventStream, Version version) => throw new NotImplementedException();
    }
}
