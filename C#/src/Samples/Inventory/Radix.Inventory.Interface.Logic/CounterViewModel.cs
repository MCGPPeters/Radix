using System.Collections.Generic;
using Radix.Components;

namespace Radix.Blazor.Inventory.Interface.Logic
{
    public class CounterViewModel : ViewModel
    {
        public int Count { get; set; }
        public IEnumerable<Error> Errors { get; set; }
    }
}
