using System;

namespace Radix
{
    /// <summary>
    ///     A reference to and identifier of an aggregate
    /// </summary>
    public record Address(Guid address) : Alias<Guid>(address)
    {
        public static implicit operator Address(Guid guid) => new(guid);
    }
}
