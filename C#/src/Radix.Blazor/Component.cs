using System;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.JSInterop;
using Radix.Blazor.Html;

namespace Radix.Blazor
{
    public abstract class Component<TViewModel, TCommand, TEvent, TFormat> : ComponentBase
        where TCommand : IComparable, IComparable<TCommand>, IEquatable<TCommand> where TEvent : class, Event

    {
        private bool _disposedValue;

        [Inject]public BoundedContext<TCommand, TEvent, TFormat> BoundedContext { get; set; }

        [Inject]public IJSRuntime JSRuntime { get; set; }
        [Inject]public NavigationManager NavigationManager { get; set; }

        [Inject]public TViewModel ViewModel { get; set; }

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
