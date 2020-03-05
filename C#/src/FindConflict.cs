using System.Collections.Generic;

namespace Radix
{
    /// <summary>
    ///     Returns the first conflict between the comment and an event, if any
    /// </summary>
    /// <param name="command"></param>
    /// <typeparam name="TCommand"></typeparam>
    /// <typeparam name="TEvent"></typeparam>
    /// <returns></returns>
    public delegate Option<Conflict<TCommand, TEvent>> FindConflict<TCommand, TEvent>(TCommand command, EventDescriptor<TEvent> eventDescriptor) where TEvent : Event;
}
