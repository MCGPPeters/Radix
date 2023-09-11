using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Radix.Interaction.Data;

namespace Radix.Interaction.Web.Components;

public abstract class Component<TModel, TCommand> : ComponentBase
{
    [Inject] protected TModel Model { get; set; } = default!;

    protected abstract Node View(TModel model, Action<TCommand> dispatch);

    protected abstract ValueTask<TModel> Update(TModel model, TCommand command);

    /// <inheritdoc />
    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        Prelude.Render(this, builder)(View(model: Model, dispatch: async command =>
        {
            Model = await Update(Model, command);
            StateHasChanged();
        }));
    }
            
}
    

