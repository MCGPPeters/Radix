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

    protected override async ValueTask<CounterModel> Update(CounterModel model, IncrementCommand command)
        {
            model.Count++;

            return model;

        }

    protected override Node View(CounterModel model, Action<IncrementCommand> dispatch) =>
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
