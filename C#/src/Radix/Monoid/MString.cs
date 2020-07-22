namespace Radix.Monoid
{


    public record MString(string Value) : Alias<string>(Value), Monoid<MString>
    {
        public MString Empty() => string.Empty;

        public static implicit operator MString(string s) => new MString(s);

        public static implicit operator string(MString mString) => mString.Value;

        public MString Append(MString x, MString y) => x + y;

    }
}
