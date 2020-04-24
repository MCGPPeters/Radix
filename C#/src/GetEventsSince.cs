using System.Collections.Generic;

namespace Radix
{
    /// <summary>
    ///     Get all event descriptors for an aggregate since (excluding) the supplied version
    /// </summary>
    /// <param name="address">State of the aggregate</param>
    /// <param name="version"></param>
    /// <typeparam name="TEvent"></typeparam>
    /// <returns></returns>
    public delegate IAsyncEnumerable<EventDescriptor<TEvent>> GetEventsSince<TEvent>(Address address, IVersion version) where TEvent : Event;
}
