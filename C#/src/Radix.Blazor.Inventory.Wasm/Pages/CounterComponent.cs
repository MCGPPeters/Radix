using System;
using System.Linq;
using Microsoft.AspNetCore.Components;
using Radix.Blazor.Html;
using Radix.Blazor.Inventory.Wasm.Pages;
using Radix.Monoid;
using Radix.Option;
using Radix.Result;
using static Radix.Blazor.Html.Elements;
using static Radix.Blazor.Html.Attributes;
using static Radix.Validated.Extensions;

namespace Radix.Blazor.Inventory.Server.Pages
{
    [Route("/counter")]
    public class CounterComponent : Component<CounterViewModel, CounterCommand, CounterEvent, Json>
    {
        private readonly Aggregate<CounterCommand, CounterEvent> _counter;

        public CounterComponent() => _counter = BoundedContext.Create(Counter.Decide, Counter.Update);


        public override Update<CounterViewModel, CounterEvent> Update { get; } = (state, @event) =>
        {
            state.Count++;
            return state;
        };

        public override Node View(CounterViewModel currentViewModel) => concat(
            h1(Enumerable.Empty<IAttribute>(), text("Counter")),
            p(Enumerable.Empty<IAttribute>(), text(ViewModel.Count.ToString())),
            button(
                new[]
                {
                    @class("btn", "btn-primary"), on.click(
                        async args =>
                        {
                            Validated<CounterCommand> validCommand = Valid(new CounterCommand());
                            Option<Radix.Error[]> result = await Dispatch(_counter, validCommand);
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

    public class CounterEvent : Event
    {
        public Address? Address { get; set; }
    }

    public class CounterCommand : IComparable, IComparable<CounterCommand>, IEquatable<CounterCommand>
    {
        public int CompareTo(object? obj) => throw new NotImplementedException();

        public int CompareTo(CounterCommand other) => throw new NotImplementedException();

        public bool Equals(CounterCommand other) => throw new NotImplementedException();
    }

}
