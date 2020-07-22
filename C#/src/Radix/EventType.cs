using System;
using Radix.Validated;

namespace Radix
{
    public record EventType : Alias<string>
    {
        public EventType(Type value) : this(value.FullName)
        {
        }

        public EventType(string value) : base(value) { }

        public static Validated<EventType> Create(string fullName) =>
            NonEmptyString
                .Create(fullName, "The name of the event can not be null or empty")
                .Map(s => new EventType(s.Value));
    }

}
