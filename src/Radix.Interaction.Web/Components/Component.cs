using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.JSInterop;

namespace Radix.Interaction.Web.Components;

public abstract class Component<TModel, TCommand> : ComponentBase
{
    [Inject] protected TModel Model { get; set; } = default!;

    protected abstract View<TModel, TCommand> View { get; }

    protected abstract Update<TModel, TCommand> Update { get; }



    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        Prelude<TModel, TCommand>.Next(Model, View, Update, Prelude.Render(this, builder), StateHasChanged);
        var foo = (string s) => (string t) => $"You passed the string {s + t}";
    }

    public Func<string, Func<string, string>> foo = (string s) => (string t) => $"You passed the string {s + t}";
}
