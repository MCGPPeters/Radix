using System;
using System.Reactive.Disposables;
using Radix.Tests.Models;

namespace Radix.Blazor.Sample
{
    public class IndexViewModel : ReadModel<IndexViewModel, InventoryItemEvent>
    {
        public IDisposable Subscribe(IObserver<IndexViewModel> observer)
        {
            return Disposable.Empty;
        }

        public bool Equals(IndexViewModel other)
        {
            return true;
        }

        public void Apply(InventoryItemEvent @event)
        {
            
        }

        public void OnCompleted()
        {
            throw new NotImplementedException();
        }

        public void OnError(Exception error)
        {
            throw new NotImplementedException();
        }

        public void OnNext(InventoryItemEvent value)
        {
            throw new NotImplementedException();
        }
    }
}
