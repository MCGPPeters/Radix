namespace Radix
{
    public readonly struct Json : Value<string>
    {
        public Json(string jsonMessage) => Value = jsonMessage;

        public string Value { get; }
    }
}
