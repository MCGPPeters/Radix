﻿using System;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.JSInterop;
using Radix.Blazor.Html;

namespace Radix.Blazor
{
    public abstract class Component<TViewModel, TCommand, TEvent> : ComponentBase, IDisposable, IObserver<TViewModel> where TEvent : Event
        where TViewModel : State<TViewModel, TEvent>, IEquatable<TViewModel>, new()
        where TCommand : IComparable, IComparable<TCommand>, IEquatable<TCommand>
    {
        private bool _disposedValue;
        private IDisposable? _subscription;

        [Inject]public BoundedContext<TCommand, TEvent> BoundedContext { get; set; }

        [Inject]public ReadModel<TViewModel, TEvent> ReadModel { get; set; }

        [Inject]public IJSRuntime JSRuntime { get; set; }


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
            Console.Out.WriteLine(OldViewModel);
            Console.Out.WriteLine(CurrentViewModel);
            //if (ShouldRender(OldViewModel, CurrentViewModel))
            //    // force render
            //    StateHasChanged();
        }

        protected override void OnInitialized()
        {
            CurrentViewModel = ReadModel.State;
            OldViewModel = CurrentViewModel;
            base.OnInitialized();
        }


        protected override void OnAfterRender(bool firstRender)
        {
            _subscription = ReadModel.Subscribe(this);
            base.OnAfterRender(firstRender);
        }

        /// <summary>
        ///     This function is called whenever it is decided the state of the viewmodel has changed
        /// </summary>
        /// <param name="context"></param>
        /// <param name="currentViewModel"></param>
        /// <returns></returns>
        public abstract Node Render(TViewModel currentViewModel);

        protected virtual bool ShouldRender(TViewModel oldViewModel, TViewModel currentViewModel)
        {
            return true;
        }


        protected override bool ShouldRender()
        {
            return ShouldRender(OldViewModel, CurrentViewModel);
        }

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            base.BuildRenderTree(builder);
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
