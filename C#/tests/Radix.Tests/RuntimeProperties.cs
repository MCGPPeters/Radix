using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Radix.Data;
using Radix.Inventory.Domain;
using Radix.Result;
using Xunit;
using static Radix.Result.Extensions;
using static Radix.Option.Extensions;

namespace Radix.Tests
{

    public class RuntimeProperties
    {

        private readonly TestSettings _testSettings = new();

        [Fact(
            DisplayName =
                "Given an instance of an aggregate is not active, but it does exist, when sending a command it should be restored and process the command")]
        public async Task Test1()
        {
            AppendEvents<Json> appendEvents = (_, __, ___, events) => Task.FromResult(Ok<ExistingVersion, AppendEventsError>(1L));

            GetEventsSince<InventoryItemEvent> getEventsSince = _testSettings.GetEventsSince;
            BoundedContext<InventoryItemCommand, InventoryItemEvent, Json> context = new(
                new BoundedContextSettings<InventoryItemEvent, Json>(
                    appendEvents,
                    getEventsSince,
                    _testSettings.CollectionSettings,
                    _testSettings.Descriptor,
                    _testSettings.SerializeEvent,
                    _testSettings.SerializeMetaData
                ));
            // for testing purposes make the aggregate block the current thread while processing
            var inventoryItem = context.Create(InventoryItem.Decide, InventoryItem.Update);
            await Task.Delay(TimeSpan.FromSeconds(1));

            Validated<InventoryItemCommand> removeItems = RemoveItemsFromInventory.Create(1, 1);

            Result<(Id, InventoryItemEvent[]), Error[]> result = await inventoryItem(removeItems);
            switch (result)
            {
                case Ok<InventoryItemEvent[], Error[]>(var events):
                    events.Should().Equal(new List<InventoryItemEvent> { new ItemsRemovedFromInventory(1, 1) });
                    break;
                case Error<InventoryItemEvent[], Error[]>(var errors):
                    errors.Should().BeEmpty();
                    break;
            }

        }
    }
}
