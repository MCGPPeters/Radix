using Radix.Data;

namespace Radix.Domain.Data;

/// <summary>
///     Create a strongly typed instance of an event given an event descriptor
/// </summary>
/// <typeparam name="TEvent"></typeparam>
/// <typeparam name="TFormat"></typeparam>
/// <param name="eventDescriptor"></param>
/// <returns></returns>
public delegate Option<TEvent> FromEventDescriptor<out TEvent, TFormat>(EventDescriptor<TFormat> eventDescriptor);
