using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Radix.Blazor.Inventory.Interface.Logic;
using Radix.Data;
using Radix.Domain.Data;
using Radix.Interaction.Data;
using Radix.Interaction.Web.Components;
using Radix.Inventory.Domain;
using Radix.Inventory.Domain.Data.Commands;
using Radix.Inventory.Domain.Data.Events;
using Radix.Tests;
using Attribute = Radix.Interaction.Data.Attribute;

namespace Radix.Inventory.Pages;

[Route("/counter")]
public class CounterComponent : Component<CounterModel, Validated<IncrementCommand>>
{
    [Inject] IJSRuntime JSRuntime { get; init; } = null!;

    protected override Interaction.Update<CounterModel, Validated<IncrementCommand>> Update =>
        async (model, command) =>
        {
            Context<IncrementCommand, CounterIncremented, InMemoryEventStore, InMemoryEventStoreSettings> context = new() { EventStoreSettings = new InMemoryEventStoreSettings() };
            var counter = await context.Create<Counter, IncrementCommand, CounterIncremented>();
            try
            {
                var result = counter.Handle(command);
            }
            catch (ValidationErrorException e)
            {
                model.Errors = e.Reasons.Select(r => new Error { Message = r.ToString() });
                await JSRuntime.InvokeAsync<string>("toast", Array.Empty<object>());
            }

            return model;

        };

    protected override Interaction.View<CounterModel, Validated<IncrementCommand>> View =>
        async (model, dispatch) =>
        await Task.FromResult(concat
        (
            new Node[]
            {
                h1
                (
                    Array.Empty<Attribute>(),
                    new[]
                    {
                        text
                        (
                            "Counter"
                        )
                    }

                ),
                p
                (
                    Array.Empty<Attribute>(),
                    new []
                    {
                        text
                        (
                            model.Count.ToString()
                        )
                    }
                ),
                button
                (

                    new[]
                    {
                            @class(new []{"btn", "btn-primary"}),
                            on.click
                            (
                                args =>
                                {
                                    Validated<IncrementCommand> validCommand = Valid(new IncrementCommand());
                                    dispatch(validCommand);
                                }
                            )


                    },
                    new []
                    {
                        text
                        (
                            "Click me"
                        )
                    }

                )
            }
        ));
}
