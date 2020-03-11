using System;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Radix.Blazor.Html;

namespace Radix.Blazor
{

    public abstract class Component<TViewModel, TCommand, TEvent> : ComponentBase, IDisposable, IObserver<TViewModel>
        where TEvent : Event 
        where TViewModel : State<TViewModel, TEvent>, IEquatable<TViewModel>, new()
    {
        [Inject]
        public BoundedContext<TCommand, TEvent> BoundedContext { get; set; }

        [Inject]
        public ReadModel<TViewModel, TEvent> ReadModel { get; set; }

        private IDisposable? _subscription;
        private bool _disposedValue;

        protected override void OnInitialized()
        {
            CurrentViewModel = ReadModel.State;
            OldViewModel = CurrentViewModel;
            _subscription = ReadModel.Subscribe(this);
        }


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


        protected abstract Node View(BoundedContext<TCommand, TEvent> boundedContext);

        protected virtual bool ShouldRender(TViewModel oldViewModel, TViewModel currentViewModel)
            => !oldViewModel.Equals(currentViewModel);


        protected override bool ShouldRender()
            => ShouldRender(OldViewModel, CurrentViewModel);

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            base.BuildRenderTree(builder);
            Rendering.RenderNode(this, builder, 0, View(BoundedContext));
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                    _subscription.Dispose();

                _disposedValue = true;
            }
        }
    }

}
