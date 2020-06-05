using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.JSInterop;
using Radix.Blazor.Html;
using Radix.Monoid;
using static Radix.Option.Extensions;
using Radix.Result;

namespace Radix.Blazor
{
    public abstract class Component<TViewModel, TCommand, TEvent, TFormat> : ComponentBase
        where TCommand : IComparable, IComparable<TCommand>, IEquatable<TCommand> where TEvent : class, Event

    {

        [Inject]public BoundedContext<TCommand, TEvent, TFormat> BoundedContext { get; set; }

        [Inject]public IJSRuntime JSRuntime { get; set; }
        [Inject]public NavigationManager NavigationManager { get; set; }

        [Inject]public TViewModel ViewModel { get; set; }


        public async Task<Option<Error[]>> Dispatch(Aggregate<TCommand, TEvent> target, Validated<TCommand> command)
        {
            Result<TEvent[], Error[]> result = await target.Accept(command);
            switch (result)
            {
                case Ok<TEvent[], Error[]>(var events):
                    ViewModel = events.Aggregate(ViewModel, (current, @event) => Update(current, @event));
                    StateHasChanged();
                    return None<Error[]>();
                case Error<TEvent[], Error[]>(var errors):
                    return Some(errors);
                default:
                    throw new ArgumentOutOfRangeException(nameof(result));
            }
        }

        //protected override bool ShouldRender() => PreviousViewModel is object && !PreviousViewModel.Equals(ViewModel);

        public abstract Update<TViewModel, TEvent> Update { get; }

        /// <summary>
        ///     This function is called whenever it is decided the state of the viewmodel has changed
        /// </summary>
        /// <param name="context"></param>
        /// <param name="currentViewModel"></param>
        /// <returns></returns>
        public abstract Node View(TViewModel currentViewModel);

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            base.BuildRenderTree(builder);

            Rendering.RenderNode(this, builder, 0, View(ViewModel));
        }
    }

}
