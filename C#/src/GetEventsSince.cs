using System.Collections.Generic;
using System.Threading.Tasks;

namespace Radix
{
    /// <summary>
    ///     Get all event descriptors for an aggregate since (excluding) the supplied version
    /// </summary>
    /// <param name="address">Address of the aggregate</param>
    /// <param name="version"></param>
    /// <typeparam name="TEvent"></typeparam>
    /// <returns></returns>
    public delegate Task<IEnumerable<EventDescriptor<TEvent>>> GetEventsSince<TEvent>(Address address, IVersion version);
}
