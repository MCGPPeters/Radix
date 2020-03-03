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
    }
}
