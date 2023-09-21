using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Radix.Blazor.Inventory.Interface.Logic;
using Radix.Interaction.Data;
using Radix.Interaction.Web.Components;

namespace Radix.Inventory.Pages;

[Route("/counter")]
public class CounterComponent : Component<CounterModel, IncrementCommand>
{
    [Inject] IJSRuntime JSRuntime { get; init; } = null!;

    public override async ValueTask<CounterModel> Update(CounterModel model, IncrementCommand command)
    {
        model.Count++;

        return model;

    }

    public override Node View(CounterModel model, Func<IncrementCommand, Task> dispatch) =>
        concat
        (
            [
                h1
                (
                    [],
                    [
                        "Counter"
                    ]

                ),
                p
                (
                    [],
                    [
                        model.Count.ToString()
                    ]
                ),
                button
                (
                   [
                            @class(new[] { "btn", "btn-primary" }),
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
