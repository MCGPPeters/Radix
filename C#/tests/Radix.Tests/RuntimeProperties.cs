using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Radix.Inventory.Domain;
using Radix.Result;
using SqlStreamStore;
using Xunit;

namespace Radix.Tests
{
    public class RuntimeProperties
    {

        [Fact(
            DisplayName =
                "Given an instance of an aggregate is not active, but it does exist, when sending a command it should be restored to the correct state and process the command")]
        public async Task Test1()
        {

            BoundedContext<InventoryItemCommand, InventoryItemEvent, Json> context = new TestInventoryBoundedContext();
            // for testing purposes make the aggregate block the current thread while processing
            var inventoryItem = context.Create<InventoryItem, InventoryItemCommandHandler>();
            await Task.Delay(TimeSpan.FromSeconds(1));

            Validated<InventoryItemCommand> removeItems = RemoveItemsFromInventory.Create(1, 1);

            Result<CommandResult<InventoryItemEvent>, Error[]> result = await inventoryItem.Accept(removeItems);
            switch (result)
            {
                case Ok<CommandResult<InventoryItemEvent>, Error[]>(var commandResult):
                    commandResult.Events.Should().BeEquivalentTo(new[] { new ItemsRemovedFromInventory(1, 1) });
                    commandResult.ExpectedVersion.Should().Be(new ExistingVersion(4L));
                    break;
                case Error<CommandResult<InventoryItemEvent>, Error[]>(var errors):
                    errors.Should().BeEmpty();
                    break;
            }

        }
    }
}
