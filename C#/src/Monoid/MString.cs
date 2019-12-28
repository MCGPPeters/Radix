namespace Radix.Monoid
{
    public readonly struct MString : Monoid<MString>
    {
        public string Value { get; }

        private MString(string value) => Value = value;


        public MString Empty() => string.Empty;

        public static implicit operator MString(string s) => new MString(s);

        public static implicit operator string(MString mString) => mString.Value;

        public MString Append(MString x, MString y) => x + y;

    }
}
