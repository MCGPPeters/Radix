using System;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.JSInterop;
using Radix.Blazor.Html;

namespace Radix.Blazor
{
    public abstract class Component<TViewModel, TCommand, TEvent> : ComponentBase, IDisposable, IObserver<TViewModel>, View where TEvent : Event
        where TViewModel : State<TViewModel, TEvent>, IEquatable<TViewModel>, new()
        where TCommand : IComparable, IComparable<TCommand>, IEquatable<TCommand>
    {
        private bool _disposedValue;
        private readonly IDisposable? _subscription;

        protected Component(BoundedContext<TCommand, TEvent> boundedContext, ReadModel<TViewModel, TEvent> readModel, IJSRuntime jsRuntime)
        {
            BoundedContext = boundedContext;
            ReadModel = readModel;
            OldViewModel = readModel.State;
            CurrentViewModel = readModel.State;
            JSRuntime = jsRuntime;
            _subscription = ReadModel.Subscribe(this);
        }

        public BoundedContext<TCommand, TEvent> BoundedContext { get; }

        public ReadModel<TViewModel, TEvent> ReadModel { get; }

        public IJSRuntime JSRuntime { get; }


        protected TViewModel OldViewModel { get; set; }
        protected TViewModel CurrentViewModel { get; set; }


        public void Dispose()
        {
            Dispose(true);
        }

        public void OnCompleted()
        {

        }

        public void OnError(Exception error)
        {

        }

        public void OnNext(TViewModel viewModel)
        {
            OldViewModel = CurrentViewModel;
            CurrentViewModel = viewModel;
            if (ShouldRender(OldViewModel, CurrentViewModel))
                // force render
                StateHasChanged();
        }

        /// <summary>
        ///     This function is called whenever it is decided the state of the viewmodel has changed
        /// </summary>
        /// <param name="context"></param>
        /// <param name="currentViewModel"></param>
        /// <returns></returns>
        public abstract Node Render(TViewModel currentViewModel);

        public  Node Render() => Render(CurrentViewModel);

        protected virtual bool ShouldRender(TViewModel oldViewModel, TViewModel currentViewModel)
        {
            return !oldViewModel.Equals(currentViewModel);
        }


        protected override bool ShouldRender()
        {
            return ShouldRender(OldViewModel, CurrentViewModel);
        }

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            base.BuildRenderTree(builder);
            if (BoundedContext is object)
                Rendering.RenderNode(this, builder, 0, Render(CurrentViewModel));
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                    DisposeManagedResources();

                _disposedValue = true;
            }
        }

        protected virtual void DisposeManagedResources()
        {

            if (_subscription is object)
                _subscription.Dispose();
        }
    }

}
