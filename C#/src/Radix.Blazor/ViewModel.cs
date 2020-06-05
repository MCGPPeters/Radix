using System.Collections.Generic;

namespace Radix.Blazor
{
    public interface ViewModel
    {
        public IEnumerable<Error> Errors { get; set; }
    }
}
