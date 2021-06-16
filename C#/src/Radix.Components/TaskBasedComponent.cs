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
    public abstract class TaskBasedComponent<TViewModel, TCommand, TEvent, TFormat> : ComponentBase
        where TViewModel : ViewModel
        where TEvent : notnull
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


        protected virtual Update<TViewModel, TEvent> Update { get; } = (state, events) => state;

        /// <summary>
        /// Execute a validated command by sending the message to an aggregate. If the command is valid, the command will be processed
        /// by the aggragate. When the command has been handled successfuly, and events are returned as a result, the events
        /// are aggragated to create the next state of the viewmodel, by calling the user defined Update function.
        /// Then a notification is sent to signal the state of the viewmodel has changed. This will trigger the component
        /// to rerender, so that it can show the correct state in the user interface. This rerendering triggers the user defined View
        /// function, which signifies how the view should be rendered
        /// </summary>
        /// <param name="target">The aggrgate that will handle the command</param>
        /// <param name="command">The validate command that the aggregate will handle, if it is valid. </param>
        /// <returns>When the command is not valid, it will return Some errors, None otherwise</returns>
        protected async Task<Option<Error[]>> Dispatch(Aggregate<TCommand, TEvent> target, Validated<TCommand> command)
        {
            Result<(Id, TEvent[]), Error[]> result = await target.Accept(command);
            switch (result)
            {
                case Ok<(Id, TEvent[]), Error[]>(var events):
                    _shouldRender = events.Item2.Any();
                    var oldState = ViewModel;
                    ViewModel = events.Item2.Aggregate(ViewModel, (current, @event) => Update(current, @event));
                    if(ViewModel != oldState)
                        StateHasChanged();
                    return None<Error[]>();
                case Error<(Id, TEvent[]), Error[]>(var errors):
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
