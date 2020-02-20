namespace Radix.Blazor
{
    public readonly struct NonNullString : Value<string>
    {
        public NonNullString(string value)
        {
            Value = value;

        }

        public string Value { get; }
    }
}
