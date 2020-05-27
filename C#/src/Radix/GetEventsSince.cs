using System.Collections.Generic;

namespace Radix
{
    /// <summary>
    ///     Get all event descriptors for an aggregate since (excluding) the supplied existingVersion
    /// </summary>
    /// <param name="address">State of the aggregate</param>
    /// <param name="version"></param>
    /// <typeparam name="TEvent"></typeparam>
    /// <typeparam name="TFormat"></typeparam>
    /// <returns></returns>
    public delegate IAsyncEnumerable<EventDescriptor<TFormat>> GetEventsSince<TFormat>(Address address, Version version, string streamId);
}
