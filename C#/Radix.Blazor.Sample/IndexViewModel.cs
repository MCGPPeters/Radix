using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Threading.Tasks;
using Radix.Tests.Models;

namespace Radix.Blazor.Sample
{

    public class IndexViewModel : State<IndexViewModel, InventoryItemEvent>, IEquatable<IndexViewModel>, IObservable<IndexViewModel>
    {
        public IndexViewModel Apply(InventoryItemEvent @event)
        {
            throw new NotImplementedException();
        }

        public bool Equals(IndexViewModel other)
        {
            throw new NotImplementedException();
        }

        public IDisposable Subscribe(IObserver<IndexViewModel> observer)
        {
            throw new NotImplementedException();
        }
    }
}
