using System;
using System.Threading.Tasks;

namespace Radix
{
    /// <summary>
    ///     Returns the uri of the resource hosting the aggregate with the indicated address
    ///     todo : will shift to the multiaddress spec https://github.com/multiformats/multiaddr for addressing
    /// </summary>
    /// <param name="address"></param>
    /// <returns>Either the Uri or an error</returns>
    public delegate Task<Result<Uri, ResolveRemoteAddressError>> ResolveRemoteAddress(Address address);
}
