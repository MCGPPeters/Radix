using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Radix.Interaction.Data;

namespace Radix.Interaction.Web;

public abstract class Component : ComponentBase
{
    public abstract Node Render();

    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        base.BuildRenderTree(builder);
        Render()(this, builder);
    }
}

public abstract class Component<TModel> : Component
    where TModel : notnull
{
    protected virtual bool ShouldRender(TModel oldModel, TModel newModel)
        => !oldModel.Equals(newModel);

    [Inject] public TModel Model { get; set; } = default!;

}

public abstract class Component<TModel, TCommand> : Component<TModel>
{
    internal TModel PreviousModel { get; set; } = default!;

    [Pure]
    public abstract Node View(TModel model, Func<TCommand, Task> dispatch);

    [Pure]
    public abstract ValueTask<TModel> Update(TModel model, TCommand command);

    protected override bool ShouldRender() =>
        ShouldRender(PreviousModel, Model);

    private async Task Dispatch(TCommand command)
    {
        Model = await Update(Model, command);
        StateHasChanged();
    }

    /// <inheritdoc />
    public override Node Render()
    {
        PreviousModel = Model;
        return View(Model, Dispatch);
    }
}


