using System.Collections.Generic;

namespace Radix.Components
{
    public record ViewModel
    {

        public IEnumerable<Error> Errors { get; set; } = new List<Error>();
    }
}
