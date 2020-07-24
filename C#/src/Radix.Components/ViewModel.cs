using System.Collections.Generic;

namespace Radix.Components
{
    public record ViewModel
    {
        // i would like this to be a init only property. However see : https://github.com/dotnet/roslyn/issues/46249.
        public IEnumerable<Error> Errors { get; set; }
    }
}
