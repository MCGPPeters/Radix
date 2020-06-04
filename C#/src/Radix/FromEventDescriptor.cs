namespace Radix
{
    /// <summary>
    ///     Create a strongly typed instance of an event given an event descriptor
    /// </summary>
    /// <typeparam name="TEvent"></typeparam>
    /// <typeparam name="TFormat"></typeparam>
    /// <param name="eventDescriptor"></param>
    /// <returns></returns>
    public delegate TEvent FromEventDescriptor<TEvent, TFormat>(Parse<TEvent, TFormat> parse, Parse<EventMetaData, TFormat> parseEventMetaData,
        EventDescriptor<TFormat> eventDescriptor);
}
