using System;
using System.Linq;
using Microsoft.AspNetCore.Components;
using Radix.Blazor.Inventory.Interface.Logic;
using Radix.Components;
using Radix.Components.Html;
using Radix.Option;
using static Radix.Components.Html.Elements;
using static Radix.Components.Html.Attributes;
using static Radix.Validated.Extensions;

namespace Radix.Blazor.Inventory.Wasm.Pages
{
    [Route("/counter")]
    public class CounterComponent : Component<CounterViewModel, CounterCommand, CounterEvent, Json>
    {
        private readonly Aggregate<CounterCommand, CounterEvent> _counter;

        public CounterComponent() => _counter = BoundedContext.Create(Counter.Decide, Counter.Update);


        protected override Update<CounterViewModel, CounterEvent> Update { get; } = (state, @event) =>
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
                            Validated<CounterCommand> validCommand = Valid(new CounterCommand());
                            Option<Error[]> result = await Dispatch(_counter, validCommand);
                            switch (result)
                            {
                                case Some<Error[]>(_):
                                    if (JSRuntime is object)
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

}
