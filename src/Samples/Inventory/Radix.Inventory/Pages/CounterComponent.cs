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
public class CounterComponent : Component<CounterModel, IncrementCommand>
{
    [Inject] IJSRuntime JSRuntime { get; init; } = null!;

    protected override Interaction.Update<CounterModel, IncrementCommand> Update =>
        async (model, command) =>
        {
            model.Count++;

            return model;

        };

    protected override Interaction.View<CounterModel, IncrementCommand> View =>
        (model, dispatch) =>
        concat
        (
            [
                h1
                (
                    [],
                    [
                        text
                        (
                            "Counter"
                        )
                    ]

                ),
                p
                (
                    [],
                    [
                        text
                        (
                            model.Count.ToString()
                        )
                    ]
                ),
                button
                (

                   [
                            @class(new []{"btn", "btn-primary"}),
                            on.click
                            (
                                args =>
                                {
                                    IncrementCommand validCommand = new IncrementCommand();
                                    dispatch(validCommand);
                                }
                            )


                    ],
                    [
                        text
                        (
                            "Click me"
                        )
                    ]
                )

                
            ]
        );
}
