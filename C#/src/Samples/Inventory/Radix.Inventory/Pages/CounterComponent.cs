using Microsoft.AspNetCore.Components;
using Radix.Blazor.Inventory.Interface.Logic;
using Radix.Components;
using Radix.Components.Html;
using Radix.Option;
using static Radix.Components.Html.Attributes;
using static Radix.Components.Html.Elements;
using static Radix.Validated.Extensions;

namespace Radix.Blazor.Inventory.Server.Pages;

[Route("/counter")]
public class CounterComponent : TaskBasedComponent<CounterViewModel, IncrementCommand, CounterIncremented, Json>
{
    protected override Update<CounterViewModel, CounterIncremented> Update { get; } = (state, @event) =>
    {
        state.Count++;
        return state;
    };


    protected override Node View(CounterViewModel currentViewModel) => concat(
        h1(Enumerable.Empty<IAttribute>(), text("Counter")),
        p(Enumerable.Empty<IAttribute>(), text(ViewModel.Count.ToString())),
        button(
            new[]
            {
                    @class("btn", "btn-primary"), on.click(
                        async args =>
                        {
                            Validated<IncrementCommand> validCommand = Valid(new IncrementCommand());
                            Aggregate<IncrementCommand, CounterIncremented>? counter = BoundedContext.Create(Counter.Decide, Counter.Update);
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

                        })
            },
            text("Click me")));
}
