using System;
using System.Linq;
using Microsoft.AspNetCore.Components;
using Radix;
using Radix.Blazor.Html;
using Radix.Monoid;
using Radix.Option;
using Radix.Result;
using static Radix.Blazor.Html.Elements;
using static Radix.Blazor.Html.Attributes;
using static Radix.Validated.Extensions;

namespace Radix.Blazor.Inventory.Server.Pages
{
    [Route("/counter")]
    public class CounterComponent : Component<CounterViewModel, IncrementCommand, CounterIncremented, Json>
    {
        private Aggregate<IncrementCommand, CounterIncremented>? _counter;


        public override Update<CounterViewModel, CounterIncremented> Update { get; } = (state, @event) =>
        {
            state.Count++;
            return state;
        };


        protected override void OnInitialized()
        {
            base.OnInitialized();
            _counter = BoundedContext.Create(Counter.Decide, Counter.Update);
        }

        public override Node View(CounterViewModel currentViewModel) => concat(
            h1(Enumerable.Empty<IAttribute>(), text("Counter")),
            p(Enumerable.Empty<IAttribute>(), text(ViewModel.Count.ToString())),
            button(
                new[]
                {
                    @class("btn", "btn-primary"), on.click(
                        async args =>
                        {
                            Validated<IncrementCommand> validCommand = Valid(new IncrementCommand());
                            if (_counter is null)
                            {
                                return;
                            }

                            Option<Radix.Error[]> result = await Dispatch(_counter, validCommand);
                            switch (result)
                            {
                                case Some<Radix.Error[]>(_):
                                    if (JSRuntime is object)
                                    {
                                        await JSRuntime.InvokeAsync<string>("toast", Array.Empty<object>());
                                    }

                                    break;
                                case None<Radix.Error[]> _:

                                    break;

                            }

                        })
                },
                text("Click me")));
    }


}
