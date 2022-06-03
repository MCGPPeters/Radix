﻿using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Radix.Blazor.Inventory.Interface.Logic;
using Radix.Data;
using Radix.Interaction.Data;
using Radix.Interaction.Web.Components;

namespace Radix.Inventory.Pages;

[Route("/counter")]
public class CounterComponent : Component<CounterModel, Validated<IncrementCommand>>
{
    [Inject] BoundedContext<IncrementCommand, CounterIncremented, Json> BoundedContext { get; set; } = null!;

    [Inject] IJSRuntime JSRuntime { get; init; } = null!;

    protected override Interaction.Update<CounterModel, Validated<IncrementCommand>> Update =>
        async (model, command) =>
        {
            var counter = BoundedContext.Create<Counter, CounterCommandHandler>();
            Result<CommandResult<CounterIncremented>, Error[]> result = await counter.Accept(command);
            switch (result)
            {
                case Error<CommandResult<CounterIncremented>, Error[]>(var errors):
                    model.Errors = errors;
                    if (JSRuntime is not null)
                    {
                        await JSRuntime.InvokeAsync<string>("toast", Array.Empty<object>());
                    }

                    break;
                case Ok<CommandResult<CounterIncremented>, Error[]>:
                    model.Count++;
                    break;
            }
            return model;
        };

    protected override Interaction.View<CounterModel, Validated<IncrementCommand>> View =>
        async (model, dispatch) =>
        concat
        (
            (NodeId)1,
            h1
            (
                (NodeId)2,
                text
                (
                    (NodeId)3,
                    "Counter"
                )
            ),
            p
            (
                (NodeId)4,
                text
                (
                    (NodeId)5,
                    model.Count.ToString()
                )
            ),
            button
            (
                (NodeId)6,
                new[]
                {
                        @class((AttributeId)7, "btn", "btn-primary"),
                        on.click
                        (
                            (AttributeId)8,
                            args =>
                            {
                                Validated<IncrementCommand> validCommand = Valid(new IncrementCommand());
                                dispatch(validCommand);
                            }
                        )
                },
                text
                (
                    (NodeId)9,
                    "Click me"
                )
            )
        );
}