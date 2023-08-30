using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Radix.Interaction.Web.Components;

public abstract class Component<TModel, TCommand> : ComponentBase
{
    [Inject] protected TModel Model { get; set; } = default!;

    protected abstract View<TModel, TCommand> View { get; }

    protected abstract Update<TModel, TCommand> Update { get; }



    protected override void BuildRenderTree(RenderTreeBuilder builder) =>
        Prelude.Render(this, builder)(View(model: Model, dispatch: async command =>
        {
            Model = await Update(Model, command);
            StateHasChanged();
        }));
}
    

