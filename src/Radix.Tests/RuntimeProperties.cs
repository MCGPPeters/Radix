using FluentAssertions;
using Radix.Data;
using Radix.Domain.Data;
using Radix.Inventory.Domain;
using Radix.Inventory.Domain.Data;
using Radix.Inventory.Domain.Data.Commands;
using Radix.Inventory.Domain.Data.Events;
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

            Context<ItemCommand, ItemEvent, Json> context = new TestInventoryBoundedContext();
            // for testing purposes make the aggregate block the current thread while processing
            var inventoryItem = context.Create<Item, ItemCommandHandler>();
            await Task.Delay(TimeSpan.FromSeconds(1));

            Validated<ItemCommand> removeItems = RemoveItemsFromInventory.Create(1, 1);

            Result<CommandResult<ItemEvent>, Error[]> result = await inventoryItem(removeItems);
            switch (result)
            {
                case Ok<CommandResult<ItemEvent>, Error[]>(var commandResult):
                    commandResult.Events.Should().BeEquivalentTo(new[] { new ItemsRemovedFromInventory(1, 1) });
                    commandResult.ExpectedVersion.Should().Be(new ExistingVersion(4L));
                    break;
                case Error<CommandResult<ItemEvent>, Error[]>(var errors):
                    errors.Should().BeEmpty();
                    break;
            }

        }
    }
}
