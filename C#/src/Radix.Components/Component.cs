using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.JSInterop;

namespace Radix.Components;

public abstract class Component<TViewModel> : ComponentBase
    where TViewModel : ViewModel
{
    [Inject] protected TViewModel ViewModel { get; set; } = default!;

    [Inject] public IJSRuntime JSRuntime { get; set; } = null!;

    [Inject] public Render Render { get; set; } = null!;
 
    /// <summary>
    ///     This function is called whenever it is decided the state of the viewmodel has changed
    /// </summary>
    /// <param name="currentViewModel"></param>
    /// <returns></returns>
    protected abstract Node View(TViewModel currentViewModel);

    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        base.BuildRenderTree(builder);
        Render(this, builder, 0, View(ViewModel));
    }
}

public abstract class Component : ComponentBase
{
    [Inject] public IJSRuntime JSRuntime { get; set; } = null!;

    [Inject] public Render Render { get; set; } = null!;

    /// <summary>
    ///     This function is called whenever it is decided the state of the viewmodel has changed
    /// </summary>
    /// <param name="currentViewModel"></param>
    /// <returns></returns>
    protected abstract Node View();

    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        base.BuildRenderTree(builder);
        Render(this, builder, 0, View());
    }
}
