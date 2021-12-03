using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.JSInterop;
using Radix.Components.Html;

namespace Radix.Components;

public abstract class Component<TViewModel> : ComponentBase
    where TViewModel : ViewModel
{
    [Inject] protected TViewModel ViewModel { get; set; } = default!;

    [Inject] public IJSRuntime JSRuntime { get; set; } = null!;

    /// <summary>
    ///     This function is called whenever it is decided the state of the viewmodel has changed
    /// </summary>
    /// <param name="currentViewModel"></param>
    /// <returns></returns>
    protected abstract Node View(TViewModel currentViewModel);

    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        base.BuildRenderTree(builder);
        Render.Node(this, builder, 0, View(ViewModel));
    }
}
