using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.JSInterop;
using Radix.Components.Html;
using Radix.Data;
using Radix.Result;
using static Radix.Option.Extensions;

namespace Radix.Components
{
    public abstract class Component<TViewModel, TCommand, TEvent, TFormat> : ComponentBase
        where TViewModel : ViewModel
        where TEvent: notnull
    {
        private bool _shouldRender;

        // ReSharper disable once MemberCanBeProtected.Global
        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        [Inject]public BoundedContext<TCommand, TEvent, TFormat> BoundedContext { get; set; } = null!;

        // ReSharper disable once MemberCanBeProtected.Global
        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        [Inject]public IJSRuntime JSRuntime { get; set; } = null!;

        // ReSharper disable once MemberCanBeProtected.Global
        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        [Inject]public NavigationManager NavigationManager { get; set; } = null!;

        // ReSharper disable once MemberCanBePrivate.Global
        [Inject]protected TViewModel ViewModel { get; set; } = default!;

        protected abstract Update<TViewModel, TEvent> Update { get; }


        protected async Task<Option<Error[]>> Dispatch(Aggregate<TCommand, TEvent> target, Validated<TCommand> command)
        {
            Result<TEvent[], Error[]> result = await target.Accept(command);
            switch (result)
            {
                case Ok<TEvent[], Error[]>(var events):
                    _shouldRender = events.Any();
                    ViewModel = events.Aggregate(ViewModel, (current, @event) => Update(current, @event));
                    StateHasChanged();
                    return None<Error[]>();
                case Error<TEvent[], Error[]>(var errors):
                    _shouldRender = true;
                    ViewModel.Errors = errors;
                    StateHasChanged();
                    return Some(errors);
                default:
                    _shouldRender = false;
                    throw new ArgumentOutOfRangeException(nameof(result));
            }
        }

        protected override bool ShouldRender() => _shouldRender;

        /// <summary>
        ///     This function is called whenever it is decided the state of the viewmodel has changed
        /// </summary>
        /// <param name="currentViewModel"></param>
        /// <returns></returns>
        protected abstract Node View(TViewModel currentViewModel);

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            base.BuildRenderTree(builder);

            Rendering.RenderNode(this, builder, 0, View(ViewModel));
        }
    }

}
