using Radix.Data;

namespace Radix;

/// <summary>
///     Returns the first conflict between the comment and an event, if any
/// </summary>
/// <param name="command"></param>
/// <typeparam name="TCommand"></typeparam>
/// <typeparam name="TEvent"></typeparam>
/// <typeparam name="TFormat"></typeparam>
/// <returns></returns>
public delegate Option<Conflict<TCommand, TEvent>> CheckForConflict<TCommand, TEvent>(TCommand command, EventDescriptor<TEvent> eventDescriptor) where TCommand : notnull where TEvent : notnull;
