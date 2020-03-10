using System;
using Radix.Tests.Models;

namespace Radix.Blazor.Sample
{

    public class IndexViewModel : State<IndexViewModel, InventoryItemEvent>, IEquatable<IndexViewModel>
    {

        public bool Equals(IndexViewModel other)
        {
            return true;
        }

        public IndexViewModel Apply(InventoryItemEvent @event)
        {
            throw new NotImplementedException();
        }
    }
}
