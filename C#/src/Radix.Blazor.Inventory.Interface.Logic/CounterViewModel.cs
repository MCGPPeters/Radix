using System.Collections.Generic;

namespace Radix.Blazor.Inventory.Interface.Logic
{
    public class CounterViewModel : ViewModel
    {
        public int Count { get; set; }
        public IEnumerable<Error> Errors { get; set; }
    }
}
