using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Radix.Blazor.Html;

namespace Radix.Blazor
{
    public abstract class BoundedContextComponent<TViewModel, TCommand, TEvent> : Component<TViewModel, TCommand, TEvent> 
        where TViewModel : ReadModel<TViewModel, TEvent>
    {
        protected BoundedContextComponent(TViewModel viewModel, BoundedContext<TCommand, TEvent> context) : base(viewModel)
        {
            _context = context;
        }

        protected BoundedContext<TCommand, TEvent> _context;

        protected override Node Render() => View(_context);
    }
}
