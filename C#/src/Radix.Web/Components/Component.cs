using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.JSInterop;

namespace Radix.Interaction.Web.Components;

public abstract class Component<TModel, TCommand> : ComponentBase
{
    [Inject] protected TModel Model { get; set; } = default!;


    protected abstract Interact<TModel, TCommand> Interact { get; }

    protected abstract Update<TModel, TCommand> Update { get; }

    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        Prelude<TModel, TCommand>.Next(Model, Interact, Update, Prelude.Render(this, builder), StateHasChanged);       
    }
}
