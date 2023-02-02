using FluentAssertions;
using Radix.Domain.Data;
using Radix.Inventory.Domain.Data;
using Radix.Inventory.Domain.Data.Commands;
using Radix.Inventory.Domain.Data.Events;
using Xunit;
using static Radix.Control.Task.Extensions;


namespace Radix.Tests;

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

        var validatedCreateItem = CreateItem.Create(1, "Product 1", true, 5);
        var validatedCheckinItems = CheckInItemsToInventory.Create(1, 19);
        var validatedRenameItem = Valid((ItemCommand)new RenameItem(1, "Product 2"));
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
