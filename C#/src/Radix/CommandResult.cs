using System.Collections.Generic;

namespace Radix
{
    public struct CommandResult<TEvent>
    {
        public Id Id { get; init; }

        public Version ExpectedVersion { get; init; }
            
        public IEnumerable<TEvent> Events { get; init; }
    }
}
