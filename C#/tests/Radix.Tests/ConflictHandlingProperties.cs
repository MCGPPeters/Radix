using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Radix.Monoid;
using Radix.Result;
using Radix.Inventory.Domain;
using Xunit;
using static Radix.Result.Extensions;

namespace Radix.Tests
{


    public class ConflictHandlingProperties
    {
        private readonly TestSettings _testSettings = new TestSettings();

        public async IAsyncEnumerable<EventDescriptor<InventoryItemEvent>> GetEventsSince(Address address, Version version, string streamIdentifier)
        {
            yield return new EventDescriptor<InventoryItemEvent>(
                new ItemsCheckedInToInventory() { Amount = 19, Id = 1 },
                2L,
                new EventType(typeof(ItemsCheckedInToInventory).FullName));
            yield return new EventDescriptor<InventoryItemEvent>(
                new InventoryItemRenamed { Id = 19, Name = "Product 2" },
                3L,
                new EventType(typeof(InventoryItemRenamed).FullName));
        }

        //[Fact(
        //    DisplayName =
        //        "Given there a technical concurrency exception arises when appending events to the stream, when the command can be applied after updating the state the expected events should be added to the stream")]
        //public async Task Property3()
        //{
        //    ExistingVersion actualVersion = 1L;
        //    AppendEvents<Json> appendEvents = (_, expectedVersion, ___, events) =>
        //    {
        //        if (expectedVersion == actualVersion)
        //        {
        //            actualVersion++;
        //            return Task.FromResult(Ok<ExistingVersion, AppendEventsError>(actualVersion));
        //        }

        //        return Task.FromResult(Error<ExistingVersion, AppendEventsError>(new OptimisticConcurrencyError("Some conflict")));
        //    };

        //    BoundedContext<InventoryItemCommand, InventoryItemEvent, Json> context = new BoundedContext<InventoryItemCommand, InventoryItemEvent, Json>(
        //        new BoundedContextSettings<InventoryItemEvent, Json>(
        //            appendEvents,
        //            GetEventsSince,
        //            _testSettings.CollectionSettings,
        //            _testSettings.Descriptor,
        //            _testSettings.ToTransientEventDescriptor,
        //            _testSettings.SerializeEvent,
        //            _testSettings.SerializeMetaData
        //        ));

        //    Aggregate<InventoryItemCommand, InventoryItemEvent> inventoryItem = context.Create(InventoryItem.Decide, InventoryItem.Update);

        //    Validated<InventoryItemCommand> checkin = CheckInItemsToInventory.Create(10);
        //    Validated<InventoryItemCommand> checkin2 = CheckInItemsToInventory.Create(2);

        //    Result<InventoryItemEvent[], Error[]> result = await inventoryItem.Accept(checkin);
        //    // simulate an additional event added to the event store
        //    actualVersion++;
        //    // another 
        //    Result<InventoryItemEvent[], Error[]> result3 = await inventoryItem.Accept(checkin2);

        //    switch (result3)
        //    {
        //        case Ok<InventoryItemEvent[], Error[]>(var events):
        //            events.Should().BeEquivalentTo(new[] {new ItemsCheckedInToInventory {Amount = 2}}, "the event should be appended");
        //            break;
        //        case Error<InventoryItemEvent[], Error[]>(var errors):
        //            errors.Should().BeEmpty();
        //            break;
        //    }
        //}

        [Fact(DisplayName = "Given there is no concurrency conflict, the expected event should be added to the stream")]
        public async Task Property5()
        {
            using BoundedContext<InventoryItemCommand, InventoryItemEvent, Json> context = new BoundedContext<InventoryItemCommand, InventoryItemEvent, Json>(
                new BoundedContextSettings<InventoryItemEvent, Json>(
                    (address, version, identifier, descriptors) => Task.FromResult(Ok<ExistingVersion, AppendEventsError>(0L)),
                    _testSettings.GetEventsSince,
                    _testSettings.CollectionSettings,
                    _testSettings.Descriptor,
                    _testSettings.SerializeEvent,
                    _testSettings.SerializeMetaData
                ));
            
                Aggregate<InventoryItemCommand, InventoryItemEvent> inventoryItem = context.Create(InventoryItem.Decide, InventoryItem.Update);

                Validated<InventoryItemCommand> create = CheckInItemsToInventory.Create(1, 15);

                Result<InventoryItemEvent[], Error[]> result = await inventoryItem.Accept(create);
                switch (result)
                {
                    case Ok<InventoryItemEvent[], Error[]>(var events):
                        events.Should().BeEquivalentTo(new ItemsCheckedInToInventory() { Amount = 15, Id = 1 });
                        break;
                    case Error<InventoryItemEvent[], Error[]>(var errors):
                        errors.Should().BeEmpty();
                        break;
                }
            
        }
    }
}
