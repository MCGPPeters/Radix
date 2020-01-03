using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Radix;
using System.Collections.Generic;

namespace Radix.Blazor
{
    public readonly struct Name : Value<string>
    {
        public Name(string v) : this() => Value = v;

        public string Value { get; }
    }

    public class Component<TState, TEvent, TCommand, TSettings> : ComponentBase, Node where TState : new()
    {
        public Aggregate<TState, TEvent, TCommand, TSettings>? Model { get; set; }
        protected override bool ShouldRender() => base.ShouldRender();
        protected override void BuildRenderTree(RenderTreeBuilder builder) => base.BuildRenderTree(builder);
    }

}