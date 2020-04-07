namespace Radix.Blazor.Html.Uri
{
    public readonly struct SchemeName : Value<string>
    {
        public SchemeName(string value)
        {
            Value = value;

        }

        public string Value { get; }
    }

    internal class Uri
    {
    }
}
