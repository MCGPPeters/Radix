using System.Collections.Generic;

namespace Radix.Components
{
    public interface ViewModel
    {
        public IEnumerable<Error> Errors { get; set; }
    }
}
