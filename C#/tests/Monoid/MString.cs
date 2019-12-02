namespace Radix.Tests.Monoid
{
    public class MString : Monoid<MString>
    {
        public string Value { get; }

        private MString(string value)
        {
            Value = value;

        }


        public MString Empty() => string.Empty;

        public static implicit operator MString(string s)
        {
            return new MString(s);
        }

        public static implicit operator string(MString mString)
        {
            return mString.Value;
        }

        public MString Append(MString x, MString y)=> x + y;

    }
}
