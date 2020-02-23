using System;
using Radix.Tests.Models;

namespace Radix.Blazor.Sample
{
    public class IndexViewModel : ReadModel<IndexViewModel, InventoryItemEvent>
    {
        public IDisposable Subscribe(IObserver<IndexViewModel> observer)
        {
            throw new NotImplementedException();
        }

        public bool Equals(IndexViewModel other)
        {
            throw new NotImplementedException();
        }

        public void Apply(InventoryItemEvent @event)
        {
            throw new NotImplementedException();
        }
    }
}
