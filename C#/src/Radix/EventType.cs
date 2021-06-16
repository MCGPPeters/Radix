using System;
using Radix.Validated;

namespace Radix
{
    public record EventType
    {
        public EventType(Type value) : this(value.FullName ?? throw new InvalidOperationException())
        {

        }

        public EventType(string value) => Value = value;

        public string Value { get; }

        public static Validated<EventType> Create(string fullName) =>
            NonEmptyString
                .Create(fullName, "The name of the event can not be null or empty")
                .Map(s => new EventType(s.Value));
    }

}
