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

        private readonly IDisposable _subscription;
        protected TViewModel currentReadModel;
        private readonly BoundedContext<TCommand, TEvent> _context;

        protected TViewModel _oldReadModel;

        private bool disposedValue;

        protected Component(ReadModel<TViewModel, TEvent> readModel, BoundedContext<TCommand, TEvent> context)
        {
            currentReadModel = readModel.State;
            _context = context;
            _oldReadModel = readModel.State;
            _subscription = readModel.Subscribe(this);
        }

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

        public void OnNext(TViewModel readModel)
        {
            _oldReadModel = currentReadModel;
            currentReadModel = readModel;
            if (ShouldRender(_oldReadModel, currentReadModel))
                // force render
                StateHasChanged();
        }


        protected abstract Node View(BoundedContext<TCommand, TEvent> boundedContext);

        protected virtual bool ShouldRender(TViewModel oldViewModel, TViewModel currentViewModel)
            => !oldViewModel.Equals(currentViewModel);


        protected override bool ShouldRender()
            => ShouldRender(_oldReadModel, currentReadModel);

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            base.BuildRenderTree(builder);
            Rendering.RenderNode(this, builder, 0, View(_context));
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                    _subscription.Dispose();

                disposedValue = true;
            }
        }
    }

}
