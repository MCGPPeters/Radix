namespace Radix
{
    public class EventType : Value<string>
    {
        public EventType(string value) => Value = value;

        public string Value { get; }
    }
}
