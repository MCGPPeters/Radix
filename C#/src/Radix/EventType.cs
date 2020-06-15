using System;
using Radix.Validated;

namespace Radix
{
    public class EventType : Value<string>
    {
        public EventType(Type value) : this(value.FullName)
        {
        }

        public EventType(string value) => Value = value;

        private static Func<string, EventType> New => name =>
            new EventType(name);

        public string Value { get; }

        public static Validated<EventType> Create(string fullName) =>
            NonEmptyString
                .Create(fullName, "The name of the event can not be null or empty")
                .Map(s => new EventType(s.Value));
    }

}
