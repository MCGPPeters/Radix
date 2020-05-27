namespace Radix
{
    public readonly struct EventType : Value<string>
    {
        public EventType(string value) => Value = value;

        public string Value { get; }
    }
}
