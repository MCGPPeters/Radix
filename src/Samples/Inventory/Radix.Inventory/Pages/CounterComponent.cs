using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Radix.Blazor.Inventory.Interface.Logic;
using Radix.Data;
using Radix.Domain.Data;
using Radix.Interaction.Data;
using Radix.Interaction.Web.Components;
using Attribute = Radix.Interaction.Data.Attribute;

namespace Radix.Inventory.Pages;

[Route("/counter")]
public class CounterComponent : Component<CounterModel, Validated<IncrementCommand>>
{
    [Inject] Context<IncrementCommand, CounterIncremented, Json> BoundedContext { get; set; } = null!;

    [Inject] IJSRuntime JSRuntime { get; init; } = null!;

    protected override Interaction.Update<CounterModel, Validated<IncrementCommand>> Update =>
        async (model, command) =>
        {
            var counter = BoundedContext.Create<Counter, CounterCommandHandler>();
            Result<CommandResult<CounterIncremented>, Error[]> result = await counter(command);
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
                            @class((NodeId)7, "btn", "btn-primary"),
                            on.click
                            (
                                (NodeId)8,
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
