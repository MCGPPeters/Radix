using System;
using System.Text;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Radix.Blazor.Html;

namespace Radix.Blazor
{

    public abstract class Component<TViewModel, TCommand, TEvent> : ComponentBase, IDisposable, IObserver<TViewModel>
        where TViewModel : ReadModel<TViewModel, TEvent> where TEvent : Event
    {
        protected Component(TViewModel viewModel)
            => _subscription = viewModel.Subscribe(this);

        protected TViewModel _oldViewModel;
        protected TViewModel _currentViewModel;

        private readonly IDisposable _subscription;


        protected abstract Node View(BoundedContext<TCommand, TEvent> boundedContext);

        protected virtual bool ShouldRender(TViewModel oldViewModel, TViewModel currentViewModel)
            => !(oldViewModel.Equals(currentViewModel));

        protected override bool ShouldRender()
            => ShouldRender(_oldViewModel, _currentViewModel);

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            base.BuildRenderTree(builder);
            Rendering.RenderNode(this, builder, 0, Render());
        }

        protected abstract Node Render();

        public void OnCompleted()
        {

        }

        public void OnError(Exception error)
        {

        }

        public void OnNext(TViewModel value)
        {
            _oldViewModel = _currentViewModel;
            _currentViewModel = value;
        }

        private bool disposedValue;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _subscription.Dispose();
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
    }

}
