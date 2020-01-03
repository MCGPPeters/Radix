namespace Radix.Blazor
{
    public readonly struct NonNullString : Value<string>
    {
        public string Value { get; }
    }
}