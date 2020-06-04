﻿using System;
using System.Linq;
using Microsoft.AspNetCore.Components;
using Radix.Blazor.Html;
using Radix.Blazor.Inventory.Interface.Logic;
using Radix.Blazor.Inventory.Wasm.Pages;
using Radix.Monoid;
using Radix.Result;
using Radix.Tests.Models;
using static Radix.Blazor.Html.Elements;
using static Radix.Blazor.Html.Attributes;
using static Radix.Validated.Extensions;

namespace Radix.Blazor.Inventory.Server.Pages
{
    [Route("/counter")]
    public class CounterComponent : Component<CounterViewModel, CounterCommand, CounterEvent, Json>
    {
        private int _currentCount;
        private Aggregate<CounterCommand, CounterEvent> _counter;

        public CounterComponent()
        {
            _counter = BoundedContext.Create(Counter.Decide, Counter.Update);

        }

        public override Node View(CounterViewModel currentViewModel) => concat(
            h1(Enumerable.Empty<IAttribute>(), text("Counter")),
            p(Enumerable.Empty<IAttribute>(), text(_currentCount.ToString())),
            button(
                new[]
                {
                    @class("btn", "btn-primary"), on.click(
                        async args =>
                        {
                            Validated<CounterCommand> validCommand = Valid(new CounterCommand());
                            Result<CounterEvent[], Radix.Error[]> result = await _counter.Accept(validCommand);
                            switch (result)
                            {
                                case Error<CounterEvent[], Radix.Error[]> error:
                                    break;
                                case Ok<CounterEvent[], Radix.Error[]> (var events):

                                    Update(ViewModel, events);

                                    break;
                                default:
                                    throw new ArgumentOutOfRangeException(nameof(result));

                            }
                        })
                },
                text("Click me")));


        public override Update<CounterViewModel, CounterEvent> Update { get; } = (state, @event) =>
        {
            state.Count++;
            return state;
        };
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
