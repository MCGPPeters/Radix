using System;
using System.Threading.Tasks;

namespace Radix
{
    /// <summary>
    ///     Forwards a command to an other context
    /// </summary>
    /// <param name="command"></param>
    /// <param name="destination"></param>
    /// <param name="address"></param>
    /// <typeparam name="TCommand"></typeparam>
    /// <returns></returns>
    public delegate Task<Result<Unit, ForwardError>> Forward<in TCommand>(TCommand command, Uri destination, Address address);
}
