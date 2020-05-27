using System.Runtime.Serialization;

namespace Radix
{
    /// <summary>
    ///     Returns the first conflict between the comment and an event, if any
    /// </summary>
    /// <param name="command"></param>
    /// <typeparam name="TCommand"></typeparam>
    /// <typeparam name="TEvent"></typeparam>
    /// <typeparam name="TFormat"></typeparam>
    /// <returns></returns>
    public delegate Option<Conflict<TCommand, TEvent>> CheckForConflict<TCommand, TEvent, TFormat>(TCommand command, EventDescriptor<TFormat> eventDescriptor);
}
