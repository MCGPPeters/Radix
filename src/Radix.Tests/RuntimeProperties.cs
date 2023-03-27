using FluentAssertions;
using Radix.Data;
using Radix.Domain.Data;
using Radix.Inventory.Domain;
using Radix.Inventory.Domain.Data;
using Radix.Inventory.Domain.Data.Commands;
using Radix.Inventory.Domain.Data.Events;
using Xunit;
using static Radix.Control.Task.Validated.Extensions;
using static Radix.Control.Validated.Extensions;


namespace Radix.Tests;

public class RuntimeProperties
{

    [Fact(
        DisplayName =
            "Given an instance of an aggregate is not active, but it does exist, when sending a command it should be restored to the correct state and process the command")]
    public async Task Test1()
    {
        Context<InventoryCommand, InventoryEvent, InMemoryEventStore, InMemoryEventStoreSettings> context = new(){EventStoreSettings = new InMemoryEventStoreSettings()};

        // for testing purposes make the aggregate block the current thread while processing
        var inventoryItem = await context.Create<Item>();

        var validatedCreateItem = CreateItem.Create(1, "Product 1", true, 5);
        var validatedCheckinItems = CheckInItemsToInventory.Create(1, 19);
        var validatedRenameItem = Valid((InventoryCommand)new RenameItem(1, "Product 2"));
        var validatedRemoveItems = RemoveItemsFromInventory.Create(1, 1);

        var x = await from instance in inventoryItem.Handle(validatedCreateItem)
            from i2 in instance.Handle(validatedCheckinItems)
            from i3 in i2.Handle(validatedRenameItem)
            from i4 in i3.Handle(validatedRemoveItems)
            select i4;

        switch (x)
        {
            case Valid<Instance<Item, InventoryCommand,  InventoryEvent>>(var
                instance):
                {
                    instance.History.Should().BeEquivalentTo(
                        new List<InventoryEvent>
                        {
                            new ItemCreated(1, "Product 1", true, 5),
                            new ItemsCheckedInToInventory {Amount = 19, Id = 1},
                            new ItemRenamed {Name = "Product 2", Id = 1},
                            new ItemsRemovedFromInventory(1, 1)
                        }, options => options.RespectingRuntimeTypes());
                    instance.Version.Should().Be(new ExistingVersion(4L));
                    instance.State.Should().BeEquivalentTo(new Item {Activated = true, Count = 23, Name = "Product 2", ReasonForDeactivation = ""});
                }
                ;
                break;
            case Invalid<Instance<Item, InventoryCommand, InventoryEvent>>(
                var invalid):
                {
                    Xunit.Assert.Fail("");
                    break;
                }
        }
    }

    [Fact(
        DisplayName =
            "Given an instance of an aggregate is not active, but it does exist, when sending a command it should be restored to the correct state and process the command")]
    public async Task Test2()
    {
        Context<InventoryCommand, InventoryEvent, InMemoryEventStore, InMemoryEventStoreSettings> context = new(){EventStoreSettings = new InMemoryEventStoreSettings()};

        // for testing purposes make the aggregate block the current thread while processing
        var inventoryItem = await context.Create<Item>();

        var validatedCreateItem = CreateItem.Create(1, "Product 1", true, 5);

        var x = await inventoryItem.Handle(validatedCreateItem);

        switch (x)
        {
            case Valid<Instance<Item, InventoryCommand,  InventoryEvent>>(var
                instance):
                {
                    instance.History.Should().BeEquivalentTo(
                        new List<InventoryEvent>
                        {
                            new ItemCreated(1, "Product 1", true, 5),
                        }, options => options.RespectingRuntimeTypes());
                    instance.Version.Should().Be(new ExistingVersion(1L));
                    instance.State.Should().BeEquivalentTo(new Item {Activated = true, Count = 5, Name = "Product 1", ReasonForDeactivation = ""});
                }
                ;
                break;
            case Invalid<Instance<Item, InventoryCommand, InventoryEvent>>(
                var invalid):
                {
                    Xunit.Assert.Fail("");
                    break;
                }
        }
    }
}

