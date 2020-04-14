using System;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.JSInterop;
using Radix.Blazor.Html;

namespace Radix.Blazor
{
    public abstract class Component<TViewModel, TCommand, TEvent> : ComponentBase where TEvent : Event
        where TViewModel : State<TViewModel, TEvent>, IEquatable<TViewModel>, new()
        where TCommand : IComparable, IComparable<TCommand>, IEquatable<TCommand>
    {
        private bool _disposedValue;

        [Inject]public BoundedContext<TCommand, TEvent> BoundedContext { get; set; }

        [Inject]public ReadModel<TViewModel, TEvent> ReadModel { get; set; }

        [Inject]public IJSRuntime JSRuntime { get; set; }


        protected TViewModel OldViewModel { get; set; }
        protected TViewModel CurrentViewModel { get; set; }

        protected override void OnInitialized()
        {
            CurrentViewModel = ReadModel.State;
            OldViewModel = CurrentViewModel;
            base.OnInitialized();
        }

        /// <summary>
        ///     This function is called whenever it is decided the state of the viewmodel has changed
        /// </summary>
        /// <param name="context"></param>
        /// <param name="currentViewModel"></param>
        /// <returns></returns>
        public abstract Node View(TViewModel currentViewModel);

        protected virtual bool ShouldRender(TViewModel oldViewModel, TViewModel currentViewModel) => true;


        protected override bool ShouldRender() => ShouldRender(OldViewModel, CurrentViewModel);

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            base.BuildRenderTree(builder);
            Rendering.RenderNode(this, builder, 0, View(CurrentViewModel));
        }
    }

}
