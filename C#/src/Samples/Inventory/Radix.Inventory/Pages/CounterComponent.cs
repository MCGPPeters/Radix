using Microsoft.AspNetCore.Components;
using Radix.Blazor.Inventory.Interface.Logic;
using Radix.Components;
using Radix.Data;

namespace Radix.Inventory.Pages;

[Route("/counter")]
public class CounterComponent : TaskBasedComponent<CounterViewModel, IncrementCommand, CounterIncremented, Json>
{
    protected override Update<CounterViewModel, CounterIncremented> Update { get; } = (state, @event) =>
    {
        state.Count++;
        return state;
    };


    protected override Node View(CounterViewModel currentViewModel) =>
    concat
    (
        h1
        (
            text
            (
                "Counter"
            )
        ),
        p
        (
            text
            (
                ViewModel.Count.ToString()
            )
        ),
        button
        (
            new[]
            {
                    @class("btn", "btn-primary"),
                    on.click
                    (
                        async args =>
                        {
                            Validated<IncrementCommand> validCommand = Valid(new IncrementCommand());
                            var counter = BoundedContext.Create<Counter, CounterCommandHandler>();
                            Option<Error[]> result = await Dispatch(counter, validCommand);
                            switch (result)
                            {
                                case Some<Error[]>(_):
                                    if (JSRuntime is not null)
                                    {
                                        await JSRuntime.InvokeAsync<string>("toast", Array.Empty<object>());
                                    }

                                    break;
                                case None<Error[]> _:

                                    break;

                            }

                        }
                    )
            },
            text
            (
                "Click me"
            )
        )
    );
}
